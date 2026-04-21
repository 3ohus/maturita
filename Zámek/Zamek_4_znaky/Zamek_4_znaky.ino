#include <Keypad.h>
#include <LiquidCrystal.h>

// --- KONFIGURACE ---
const byte POCET_RADKU = 4;
const byte POCET_SLOUPCU = 4;
const byte DELKA_KODU = 4;

// ZDE ZMĚŇ HESLO (může obsahovat čísla i písmena A, B, C, D)
const char HESLO[DELKA_KODU + 1] = "1A2B"; 

const int PIN_RELE = A4; 
const int DOBA_SEPNUTI = 3000; 

char klavesy[POCET_RADKU][POCET_SLOUPCU] = {
  {'1', '2', '3', 'A'},
  {'4', '5', '6', 'B'},
  {'7', '8', '9', 'C'},
  {'*', '0', '#', 'D'}
};

byte pinyRadku[POCET_RADKU] = {A3, A2, A1, A0};
byte pinySloupcu[POCET_SLOUPCU] = {7, 8, 9, 10};

Keypad klavesnice = Keypad(makeKeymap(klavesy), pinyRadku, pinySloupcu, POCET_RADKU, POCET_SLOUPCU);
const int rs = 12, en = 11, d4 = 2, d5 = 3, d6 = 4, d7 = 5;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

// --- SYSTÉMOVÉ PROMĚNNÉ ---
char bufferVstupu[DELKA_KODU]; 
byte indexVstupu = 0;

void setup() {
  Serial.begin(9600);
  lcd.begin(16, 2);
  
  pinMode(PIN_RELE, OUTPUT);
  digitalWrite(PIN_RELE, LOW);

  vypisUvodniObrazovku();
}

void loop() {
  char klavesa = klavesnice.getKey();

  if (klavesa) {
    zpracujStisk(klavesa);
  }
}

void zpracujStisk(char znak) {
  // Resetování zadávání
  if (znak == '*') {
    resetujSystem();
    return;
  }

  // Potvrzení hesla (Enter)
  if (znak == '#') {
    if (indexVstupu == DELKA_KODU) {
      overHeslo();
    }
    return;
  }

  // Sběr znaků (včetně písmen A-D)
  if (indexVstupu < DELKA_KODU) {
    bufferVstupu[indexVstupu] = znak;
    
    lcd.setCursor(6 + indexVstupu, 1);
    lcd.print('*'); // Maskování
    indexVstupu++;
  }
}

void overHeslo() {
  bool shodujeSe = true;
  for (byte i = 0; i < DELKA_KODU; i++) {
    if (bufferVstupu[i] != HESLO[i]) {
      shodujeSe = false;
      break; 
    }
  }

  lcd.clear();
  if (shodujeSe) {
    lcd.setCursor(0, 0);
    lcd.print(">> OTEVRENO <<");
    lcd.setCursor(0, 1);
    
    // Aktivace relé
    digitalWrite(PIN_RELE, HIGH);
    delay(DOBA_SEPNUTI);
    digitalWrite(PIN_RELE, LOW);
    
    resetujSystem();
  } else {
    lcd.setCursor(0, 0);
    lcd.print("!! CHYBA !!");
    lcd.setCursor(0, 1);
    lcd.print("Spatne heslo");
    
    delay(2000); 
    resetujSystem(); 
  }
}

void vypisUvodniObrazovku() {
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Zadej heslo:");
  lcd.setCursor(6, 1);
  lcd.print("____");
}

void resetujSystem() {
  indexVstupu = 0;
  digitalWrite(PIN_RELE, LOW);
  vypisUvodniObrazovku();
}