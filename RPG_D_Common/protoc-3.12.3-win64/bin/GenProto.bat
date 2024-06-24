protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../RPG_D_Server/PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../RPG_D_Client/Assets/MyResource/Script/Packet"
XCOPY /Y Protocol.cs "../../../RPG_D_Server/Server/Packet"

echo RPG_D Protocol generatnig completed.