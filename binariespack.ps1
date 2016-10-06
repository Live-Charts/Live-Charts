$as = ".\binaries\net403", ".\binaries\net45", ".\binaries\net451", ".\binaries\net452", ".\binaries\net46", ".\binaries\net461"

ForEach ($a in $as) {
    if(!(Test-Path -Path $a )){
        New-Item -ItemType directory -Path $a
    }
    $p = $a + "/*.*"
    Remove-Item $p
}

$targets = "net403", "net45", "net451", "net452", "net46", "net461"

ForEach ($t in $targets){
    Write-Host $t"..."
    $from = ".\WinFormsView\bin\" + $t + "\LiveCharts.dll";
    $to = ".\binaries\" + $t
    Copy-Item $from $to
    $from = ".\WinFormsView\bin\" + $t + "\LiveCharts.Wpf.dll";
    Copy-Item $from $to
    $from = ".\WinFormsView\bin\" + $t + "\LiveCharts.WinForms.dll";
    Copy-Item $from $to
}

