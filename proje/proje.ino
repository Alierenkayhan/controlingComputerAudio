int increaseAudioState = 0;
int decreaseAudioState = 0;
int video1State = 0;
int video2State = 0;

int increaseAudio = 2;
int decreaseAudio = 3;
int video1 = 4;
int video2 = 5;

void setup()
{
  Serial.begin(9600);
  pinMode(increaseAudio, INPUT);
  pinMode(decreaseAudio, INPUT);
  pinMode(video1, INPUT);
  pinMode(video2, INPUT);
}

void loop()
{
  increaseAudioState = digitalRead(2);
  decreaseAudioState = digitalRead(3);
  video1State = digitalRead(4);
  video2State = digitalRead(5);

  if (increaseAudioState == HIGH) 
    Serial.println("1");
  if (decreaseAudioState == HIGH) 
    Serial.println("2");
  if (video1State == HIGH) 
    Serial.println("3");
  if (video2State == HIGH) 
    Serial.println("4");
    
  delay(100);
}