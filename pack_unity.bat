@echo off
setlocal

REM 実行フォルダ名（プロジェクト名）を取得
for %%i in (.) do set PROJECT_NAME=%%~nxi

REM 圧縮ファイル名（タイムスタンプなし）
set ZIPNAME=%PROJECT_NAME%.zip

REM 一時ファイルリスト作成
echo Assets> filelist.txt
echo Packages>> filelist.txt
echo ProjectSettings>> filelist.txt
echo %~nx0>> filelist.txt

REM 圧縮（PowerShellのCompress-Archiveを使用）
powershell -Command "Compress-Archive -Path (Get-Content filelist.txt) -DestinationPath %ZIPNAME% -Force"

REM 一時ファイル削除
del filelist.txt

echo 圧縮完了: %ZIPNAME%

endlocal
pause