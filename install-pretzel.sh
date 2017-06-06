rm -rf pretzel
git clone -b custom https://github.com/AndreyAkinshin/pretzel.git
wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -O pretzel/src/nuget.exe
mono pretzel/src/nuget.exe restore pretzel/src/
msbuild /p:Configuration=Release pretzel/src/
rm -rf bin
cp -a pretzel/src/Pretzel/bin/Release/ bin/