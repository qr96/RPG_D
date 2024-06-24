@echo off
scp -i "C:\Users\Lee\Desktop\Keys\Yatzy\NHN Cloud Key/yatzyKey.pem" -r ..\Server\bin\Debug\net7.0\linux-x64\publish centos@180.210.82.243:~/test
echo RPG_C Server publish complted.
pause