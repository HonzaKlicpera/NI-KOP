$workfolder = "C:\\Users\\Johny\\source\\repos\\NI-KOP\\ReferenceData\\HW2"
$appFolder = "C:\\Users\\Johny\\Desktop\\KOPapp\\KnapsackProblem.exe"
$maxInstanceSize = 40
$minInstanceSize = 1
$strategies = "FPTAS"
$datasets = "NK", "ZKW", "ZKC"
$datasets | ForEach-Object -Process {
	$dataset = $_
	$strategies | ForEach-Object -Process {
		$dataFiles = "$workfolder"+"\\"+"$dataset"+"\\*_inst.dat"
		$outputFolder = "$workfolder"+"\\results.csv"
		$strategy = $_
		Get-ChildItem "$dataFiles" -Name | ForEach-Object -Process {
			$instanceSize = "$_" -replace "[^0-9]";
			If(([int]$instanceSize -le [int]$maxInstanceSize) -and ([int]$instanceSize -ge [int]$minInstanceSize) ) {
				$testFile = $dataset+$instanceSize+"_sol.dat"
				$inputFile = "$workfolder"+"\\"+$dataset+"\\"+"$_"
				$testfile = "$workfolder"+"\\"+$dataset+"\\"+$testFile
				& $appFolder -i "$inputFile" -p constructive -t "$testFile" -s $strategy --aprAccuracy 0.05 -o "$outputFolder" --setname=$dataset
			}
		}
	}
}