// Sample code for unzipping a Feb 2024 Wiki dump page, extracting the metadata and deleting the ZIP file.

using System.Diagnostics;
using Extract;

Console.WriteLine("Args:");
foreach (var arg in args)
{
    Console.WriteLine(arg);
}

string filename = args[0];
//string filename = "enwiki-20240201-pages-meta-history9.xml-p4034021p4045402";
string zipFolder = "C:\\dev\\qut\\IFN704";
string workingFolder = "F:\\";

string zippedFile = Path.Combine(zipFolder, filename + ".7z");
string unzippedFile = Path.Combine(workingFolder, filename);

static void Unzip(string zipFile, string workingFolder)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {

            FileName = "7z",
            ArgumentList = { "-y", "e", zipFile, "-o" + workingFolder }
        }
    };
    process.Start();
    process.WaitForExit();
}

// Unzip archive
Console.WriteLine($"Unzipping: {zippedFile}");
Unzip(zippedFile, workingFolder);
Console.WriteLine($"Finished Unzipping");

string outRevisionsFile = Path.Combine(workingFolder, $"{filename}_revisions.txt");
string outPagesFile = Path.Combine(workingFolder, $"{filename}_pages.txt");

// Extract metadata
var p = new Parser();
var start = DateTime.Now;
p.Run(unzippedFile, outRevisionsFile, outPagesFile);
var end = DateTime.Now;

// Delete unzipped file
File.Delete(unzippedFile);

Console.WriteLine($"{(end-start).TotalSeconds} seconds");




