rmdir "en/_site" /s /q
xcopy "_files/data" "en/_site/data" /i /s /y
xcopy "_files/en" "en/_site/en" /i /s /y
bin\Pretzel.exe taste en