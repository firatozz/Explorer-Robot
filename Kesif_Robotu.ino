#include <Servo.h>

Servo x;
Servo y;
int sag_i=8;//in1
int sag_g=11;//in2
int sol_i=12;//in3
int sol_g=13;//in4

int ea=9;
int eb=10;

int veri [4];

void setup() {
  veri[3]=150; 
  x.attach(7); 
  y.attach(6); 
  analogWrite(ea,200);
  analogWrite(eb,200);
  pinMode(sag_i,OUTPUT);
  pinMode(sag_g,OUTPUT);
  pinMode(sol_i,OUTPUT);
  pinMode(sol_g,OUTPUT);
  Serial.begin(9600);
  
}

void loop() {
if (Serial.available()>0){

for(int i=0;i<4;i++){
   veri[i]=Serial.read();
}
veri[3]=map(veri[3],0,100,0,255);
     analogWrite(ea,veri[3]);
 analogWrite(eb,veri[3]);
 veri[1]=map(veri[1],0,100,10,170);
 veri[2]=map(veri[2],0,100,10,130);
 
  Serial.print(veri[0]);
    Serial.print("    ");
  Serial.print(veri[1]);
  Serial.print("    ");
  Serial.print(veri[2]);
    Serial.print("    ");
  Serial.println(veri[3]);

     if(veri[0]==1){
      ileri();
      }else  if(veri[0]==2){
      geri();
      }else  if(veri[0]==3){
      sag();
      }else  if(veri[0]==4){
      sol();
      }
      else  if(veri[0]==0){
      dur();
      }

    x.write(veri[1]);       
 y.write(veri[2]);   
  }
  
delay(200);
}


  void ileri(){
  digitalWrite(sag_i,HIGH);
  digitalWrite(sag_g,LOW);
  digitalWrite(sol_i,HIGH);
  digitalWrite(sol_g,LOW);
  }
  
  void geri(){
  digitalWrite(sag_g,HIGH);
  digitalWrite(sag_i,LOW);
  digitalWrite(sol_g,HIGH);
  digitalWrite(sol_i,LOW);
  }
  
   void sag(){
  digitalWrite(sag_g,HIGH);
  digitalWrite(sag_i,LOW);
  digitalWrite(sol_i,HIGH);
  digitalWrite(sol_g,LOW);
  }
  void sol(){
  digitalWrite(sag_i,HIGH);
  digitalWrite(sag_g,LOW);
  digitalWrite(sol_g,HIGH);
  digitalWrite(sol_i,LOW);
  }
  
  void dur(){
    digitalWrite(sag_i,LOW);
  digitalWrite(sag_g,LOW);
  digitalWrite(sol_g,LOW);
  digitalWrite(sol_i,LOW);
    }
