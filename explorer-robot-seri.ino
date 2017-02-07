#include <Servo.h>
int MotorSol1 = 8;  //IN1
int MotorSol2 = 11; //IN2
int MotorSag1 = 12;  //IN3
int MotorSag2 = 13;  //IN4
int EA = 9;    //enable out1
int EB = 10;   //enable out2

//Motor Durumları
uint8_t MotorSol1Durum = LOW;
uint8_t MotorSol2Durum = LOW;
uint8_t MotorSag1Durum = LOW;
uint8_t MotorSag2Durum = LOW;

Servo servoX;
Servo servoY;

byte serialIn = 0;
byte commandAvailable = false;
String strReceived = "";


byte servoXMerkezNoktasi = 94;
byte servoYMerkezNoktasi = 90;

// iki servonun maksimum açısı
byte servoXmax = 170;
byte servoYmax = 130;

// iki servonun minimum açısı
byte servoXmin = 10;
byte servoYmin = 10;

//İki servonun geçerli açısı
byte servoXNoktasi = 0;
byte servoYNoktasi = 0;

// motorların geçerli hızı
byte solhiz = 0;
byte saghiz = 0;

byte servoAdim = 4;


void setup()
{
  servoX.attach(6);//servo 1
  servoY.attach(7);//servo 2

  pinMode(MotorSol1, OUTPUT);
  pinMode(MotorSol2, OUTPUT);
  pinMode(MotorSag1, OUTPUT);
  pinMode(MotorSag2, OUTPUT);
  servo_test();//test servo
  Serial.begin(9600);
}


void loop()
{
  getSerialLine();
  if (commandAvailable) {
    processCommand(strReceived);
    strReceived = "";
    commandAvailable = false;
  }
}


void getSerialLine()
{
  while (serialIn != '\r')
  {
    if (!(Serial.available() > 0))
    {
      return;
    }

    serialIn = Serial.read();
    if (serialIn != '\r') {
      if (serialIn != '\n') {
        char a = char(serialIn);
        strReceived += a;
      }
    }
  }
  if (serialIn == '\r') {
    commandAvailable = true;
    serialIn = 0;
  }
}

void processCommand(String input)
{
  String command = getValue(input, ' ', 0);
  byte iscommand = true;
  int val;
  if (command == "komut_ileri")
  {
    ileri();
  }
  else if (command == "komut_geri")
  {
    geri();
  }
  else if (command == "komut_sol")
  {
    sol();
  }
  else if (command == "komut_sag")
  {
    sag();
  }
  else if (command == "komut_dur")
  {
    dur();
  }
  else if (command == "komut1")
  {
    val = getValue(input, ' ', 1).toInt();
    solhiz = val;
    val = getValue(input, ' ', 2).toInt();
    saghiz = val;
  }
  else if (command == "komut_servo_test")
  {
    servo_test();
  }
  else if (command == "komut_servo_yukari")
  {
    servo_yukari();
  }
  else if (command == "komut_servo_asagi")
  {
    servo_asagi();
  }
  else if (command == "komut_servo_sol")
  {
    servo_sol();
  }
  else if (command == "komut_servo_sag")
  {
    servo_sag();
  }
  else if (command == "komut_servo_merkez")
  {
    servo_merkez();
  }
  else if (command == "servo_dikey")// servo dikey döndürme
  {
    val = getValue(input, ' ', 1).toInt();
  }
  else if (command == "servo_yatay")//servo yatay döndürme
  {
    val = getValue(input, ' ', 1).toInt();
  }
  else
  {
    iscommand = false;
  }
  if (iscommand) {
    SendMessage("cmd:" + input);
    SendStatus();
  }

}
String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {
    0, -1
  };
  int maxIndex = data.length() - 1;

  for (int i = 0; i <= maxIndex && found <= index; i++) {
    if (data.charAt(i) == separator || i == maxIndex) {
      found++;
      strIndex[0] = strIndex[1] + 1;
      strIndex[1] = (i == maxIndex) ? i + 1 : i;
    }
  }

  return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}
void servo_test(void) {
   int simdikikoseY = servoY.read();
  int simdikikoseX = servoX.read();
  servo_Dikey(servoYmin);
  delay(500);
  servo_Dikey(servoYmax);
  delay(500);
  servo_Dikey(servoYMerkezNoktasi);
  delay(500);
  servo_Yatay(servoXmin);
  delay(500);
  servo_Yatay(servoXmax);
  delay(500);
  servo_Yatay(servoXMerkezNoktasi);
  delay(500);
  servo_merkez();
}
void servo_sag(void)
{
  int servotemp = servoX.read();
  servotemp -= servoAdim;
  servo_Yatay(servotemp);
}

void servo_sol(void)
{
  int servotemp = servoX.read();
  servotemp += servoAdim;
  servo_Yatay(servotemp);
}

void servo_asagi(void)
{
  int servotemp = servoY.read();
  servotemp += servoAdim;
  servo_Dikey(servotemp);
}

void servo_yukari(void)
{
  int servotemp = servoY.read();
  servotemp -= servoAdim;
  servo_Dikey(servotemp);
}

void servo_merkez(void)
{
  servo_Dikey(servoYMerkezNoktasi);
  servo_Yatay(servoXMerkezNoktasi);
}

void servo_Dikey(int kose)
{
  int koseY = servoY.read();
  if (koseY > kose)
  {
    for (int i = koseY; i > kose; i - servoAdim)
    {
      servoY.write(i);
      servoYNoktasi = i;
      delay(50);
    }
  }
  else
  {
    for(int i = koseY; i < kose; i + servoAdim)
    {
      servoY.write(i);
      servoYNoktasi = i;
      delay(50);
    }
  }
  servoY.write(kose);
  servoYNoktasi = kose;
}

void servo_Yatay(int kose)  // Kendime not : servo_dikey ile karşılaştır, farklara bak.
{
  int i = 0;
  byte koseX = servoX.read();
  if (koseX > kose)
  {
    for ( i = koseX; i > kose; i - servoAdim)
    {
      servoX.write(i);
      servoXNoktasi = i;
      delay(50);
    }
  }
  else
  {
    for (i = koseX; i < kose; i = i + servoAdim)
    {
      servoX.write(i);
      servoXNoktasi = i;
      delay(50);
    }
  }
  servoX.write(kose);
  servoXNoktasi = kose;
}


void ileri(void)
{
  MotorSol1Durum = LOW;
  MotorSol2Durum = HIGH;
  MotorSag1Durum = LOW;
  MotorSag2Durum = HIGH;
  SetEN();
}
void geri(void)
{
  MotorSol1Durum = HIGH;
  MotorSol2Durum = LOW;
  MotorSag1Durum = HIGH;
  MotorSag2Durum = LOW;
  SetEN();
}
void sag(void)
{
  MotorSol1Durum = LOW;
  MotorSol2Durum = HIGH;
  MotorSag1Durum = HIGH;
  MotorSag2Durum = LOW;
  SetEN();
}
void sol(void)
{
  MotorSol1Durum = HIGH;
  MotorSol2Durum = LOW;
  MotorSag1Durum = LOW;
  MotorSag2Durum = HIGH;
  SetEN();
}
void dur(void)
{
  solhiz = 0;
  saghiz = 0;
  MotorSol1Durum = LOW;
  MotorSol2Durum = LOW;
  MotorSag1Durum = LOW;
  MotorSag2Durum = LOW;
  SetEN();
}

void SetEN() {
  analogWrite(EA, solhiz);
  analogWrite(EB, saghiz);
  digitalWrite(MotorSol1, MotorSol1Durum);
  digitalWrite(MotorSol2, MotorSol2Durum);
  digitalWrite(MotorSag1, MotorSag1Durum);
  digitalWrite(MotorSag2, MotorSag2Durum);
}

void SendStatus() {

  String out = "";
  out += MotorSol1Durum;
  out += ",";
  out += MotorSol2Durum;
  out += ",";
  out += MotorSag1Durum;
  out += ",";
  out += MotorSag2Durum;
  out += ",";
  out += solhiz;
  out += ",";
  out += saghiz;
  out += ",";
  out += servoXNoktasi;
  out += ",";
  out += servoYNoktasi;
  SendMessage(out);
}
void SendMessage(String data) {
  Serial.println(data);
}

