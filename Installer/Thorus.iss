;------------------------------
; macrodefinitions
#define BINDIR "..\bin"
#define OUTDIR "..\output"

#define VERSION GetStringFileInfo(BINDIR + "\ThorusCommon.dll", FILE_VERSION)
#define COMPANY GetStringFileInfo(BINDIR + "\ThorusCommon.dll", COMPANY_NAME)
#define PRODUCT GetStringFileInfo(BINDIR + "\ThorusCommon.dll", PRODUCT_NAME)
#define REGENTRY "Software" + "\" + COMPANY + "\" + PRODUCT

[Setup]
;------------------------------
AppId={{CEC31B9A-34ED-450F-AF76-5D8827EBFD44}
;------------------------------
AppVersion={#VERSION}
OutputBaseFilename={#PRODUCT} {#VERSION}
VersionInfoVersion={#VERSION}
VersionInfoTextVersion={#VERSION}
VersionInfoProductVersion={#VERSION}

AppPublisher={#COMPANY}
VersionInfoCompany={#COMPANY}
VersionInfoCopyright={#COMPANY}

AppName={#PRODUCT}
VersionInfoDescription={#PRODUCT}
VersionInfoProductName={#PRODUCT}

DefaultDirName={pf}\{#COMPANY}\{#PRODUCT}
DefaultGroupName={#COMPANY}\{#PRODUCT}

OutputDir={#OUTDIR}

;------------------------------
AllowNoIcons=true
PrivilegesRequired=none
ChangesEnvironment=false
Compression=lzma
DirExistsWarning=yes
DisableFinishedPage=false
DisableReadyPage=false
DisableStartupPrompt=true
EnableDirDoesntExistWarning=true
LanguageDetectionMethod=locale

; Minimum Windows 10 1607 (because of .NET 8)
MinVersion=0,10.0.14393

SetupLogging=true
ShowLanguageDialog=yes
ShowTasksTreeLines=true
SignedUninstaller=false
SolidCompression=true
UninstallLogMode=append
UsePreviousLanguage=no
DisableWelcomePage=False
AllowUNCPath=False

[Tasks]

[Files]
Source: "vc_redist.x64.exe"; DestDir: {tmp}; Flags: deleteafterinstall
Source: "{#BINDIR}\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\*.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\*.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\Data\*"; DestDir: "{app}\Data"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "{#BINDIR}\Images\*"; DestDir: "{app}\Images\Palettes"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "{#BINDIR}\Palettes\*"; DestDir: "{app}\Palettes"; Flags: ignoreversion createallsubdirs recursesubdirs
Source: "{#BINDIR}\runtimes\*"; DestDir: "{app}\runtimes"; Flags: ignoreversion createallsubdirs recursesubdirs

[Icons]
Name: "{group}\Thorus Weather Studio"; Filename: "{app}\ThorusViewer.exe"; WorkingDir: "{app}"; IconFilename: "{app}\ThorusViewer.exe"

[Types]

[Components]

[Run]
Filename: {tmp}\vc_redist.x64.exe; Parameters: "/install /quiet /norestart"; StatusMsg: "Installing VC++ 2015 Redistributables..."

[UninstallRun]

[Registry]
