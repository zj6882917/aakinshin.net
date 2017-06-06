rmdir "pretzel" /s /q
git clone -b custom https://github.com/AndreyAkinshin/pretzel.git
nuget restore pretzel\src\
MSBuild /p:Configuration=Release pretzel\src\
rmdir "bin" /s /q
xcopy "pretzel/src/Pretzel/bin/Release" "bin" /i /s /y