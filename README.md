# RFID-Reader
Notes before beginning:
-	Ensure the serial port is enabled on the raspberry

# If you receive errors in the bin or obj directory:
-	The line in the csproj file <GenerateAssemblyInfo>false</GenerateAssemblyInfo> will prevent it from reoccurring
-	Delete the bin and obj directories from the project
-	In terminal run:  dotnet restore
-	If prompted by VSCode, create a new folder for the deleted directories.

# Transferring to the pi:
-	Create a folder on the pi for the transfer 
-	Run:  dotnet publish -r linux-arm **
-	Use:  scp -r bin/Debug/netcoreapp2.2/linux-arm/publish pi@[the pi IP address]:~/[the directory pointing to the file]
-	Change the permissions of the executable on the pi:  chmod u+x RFID
-	Run on the pi:  sudo ./RFID

# ** After the first scp is formed, it will deploy to a folder in the directory named “publish”.  Repeat the chmod on the new executable and run from the publish folder.  I’m still trying to figure out how to correct this.
