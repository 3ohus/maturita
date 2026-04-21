#include <LiquidCrystal.h>
#include <EEPROM.h>
#include <avr/interrupt.h>

const int rs = 12, en = 11, d4 = 2, d5 = 3, d6 = 4, d7 = 5;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

const int pinMereni = 6; 
const int pinReset = 7;

unsigned long celkoveSekundy = 0;
volatile bool tik = false;

void uloz() {
  unsigned int h = celkoveSekundy / 3600;
  unsigned int m = (celkoveSekundy % 3600) / 60;
  EEPROM.put(10, h);
  EEPROM.put(14, m);
}

void nacti() {
  unsigned int h, m;
  EEPROM.get(10, h);
  EEPROM.get(14, m);
  if (h > 60000) h = 0;
  if (m > 59) m = 0;
  celkoveSekundy = (unsigned long)h * 3600 + m * 60;
}

void setup() {
  lcd.begin(16, 2);
  pinMode(pinMereni, INPUT_PULLUP);
  pinMode(pinReset, INPUT_PULLUP);
  nacti();
  
  cli();
  TCCR1A = 0; TCCR1B = 0; TCNT1 = 0; OCR1A = 15624;
  TCCR1B |= (1 << WGM12) | (1 << CS12) | (1 << CS10);
  TIMSK1 |= (1 << OCIE1A);
  sei();
}

ISR(TIMER1_COMPA_vect) { tik = true; }

void loop() {
  if (digitalRead(pinReset) == LOW) {
    celkoveSekundy = 0;
    uloz();
    lcd.clear();
    while(digitalRead(pinReset) == LOW);
  }

  if (tik) {
    tik = false;
    if (digitalRead(pinMereni) == LOW) {
      celkoveSekundy++;
      if (celkoveSekundy % 600 == 0) uloz(); // 10 minut = 600s
    }

    unsigned int h = celkoveSekundy / 3600;
    unsigned int m = (celkoveSekundy % 3600) / 60;

    lcd.setCursor(0, 0);
    if (h < 10) lcd.print("0"); lcd.print(h);
    lcd.print(":");
    if (m < 10) lcd.print("0"); lcd.print(m);
    
    if (h >= 150) lcd.print(" !!"); else lcd.print("   ");
  }
}