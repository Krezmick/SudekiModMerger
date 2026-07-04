using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace SudekiModToolGUI
{
    public partial class Form1 : Form
    {
        enum MergeMode
        {
            AbortOnConflict,
            ModA,
            ModB
        }

        public Form1()
        {
            InitializeComponent();
            string exeDir = AppDomain.CurrentDomain.BaseDirectory; //Set background img
            string bgPath = Path.Combine(exeDir, "sudekimodmergebg.jpg");

            if (File.Exists(bgPath))
            {
                this.BackgroundImage = Image.FromFile(bgPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void btnCheckConflicts_Click(object sender, EventArgs e)
        {
            RunMerger(checkOnly: true);
        }

        private void btnMergeMods_Click(object sender, EventArgs e)
        {
            RunMerger(checkOnly: false);
        }

        private void RunMerger(bool checkOnly)
        {
            txtOutput.Clear();

            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string originalFile = Path.Combine(exeDir, "SOLData.baf");
            string fileA = Path.Combine(exeDir, "SOLData_A.baf");
            string fileB = Path.Combine(exeDir, "SOLData_B.baf");
            string mergedFile = Path.Combine(exeDir, "SOLDataMerged.baf");

            if (!File.Exists(originalFile) || !File.Exists(fileA) || !File.Exists(fileB)) //Make sure we have files
            {
                txtOutput.Text = "Missing required .baf files (SOLData.baf, SOLData_A.baf & SOLData_B.baf).";
                return;
            }

            MergeMode mode = MergeMode.AbortOnConflict;

            if (checkOnly)
            {
                txtOutput.AppendText("Checking for conflicts, please wait...\r\n\r\n");
            }
            else
            {
                DialogResult choice = MessageBox.Show(
                    "Choose conflict resolution:\n\nYes = Mod A\nNo = Mod B\nCancel = Abort on conflict",
                    "Conflict Resolution",
                    MessageBoxButtons.YesNoCancel);

                if (choice == DialogResult.Yes)
                    mode = MergeMode.ModA;
                else if (choice == DialogResult.No)
                    mode = MergeMode.ModB;
                else
                    mode = MergeMode.AbortOnConflict;

                txtOutput.AppendText("Merging files, please wait...\r\n\r\n");
            }

            long sizeO = new FileInfo(originalFile).Length;
            long sizeA = new FileInfo(fileA).Length;
            long sizeB = new FileInfo(fileB).Length;

            long max = Math.Max(sizeO, Math.Max(sizeA, sizeB));

            using (var mmO = MemoryMappedFile.CreateFromFile(originalFile)) //Begin comparison by streaming the file data
            using (var mmA = MemoryMappedFile.CreateFromFile(fileA))
            using (var mmB = MemoryMappedFile.CreateFromFile(fileB))
            using (var viewO = mmO.CreateViewAccessor())
            using (var viewA = mmA.CreateViewAccessor())
            using (var viewB = mmB.CreateViewAccessor())
            using (var outStream = checkOnly ? Stream.Null : new FileStream(mergedFile, FileMode.Create, FileAccess.Write))
            {
                bool conflictFound = false;
                int conflictCount = 0;

                for (long i = 0; i < max; i++)
                {
                    byte o = (i < sizeO) ? viewO.ReadByte(i) : (byte)0;
                    byte a = (i < sizeA) ? viewA.ReadByte(i) : (byte)0;
                    byte b = (i < sizeB) ? viewB.ReadByte(i) : (byte)0;

                    bool aChanged = a != o;
                    bool bChanged = b != o;

                    byte result; //byte data to be merged

                    if (aChanged && bChanged) //Confilct logic
                    {
                        if (a == b)
                        {
                            result = a; //same byte = safe
                        }
                        else
                        {
                            conflictFound = true;
                            conflictCount++; 

                            if (checkOnly)
                            {
                                txtOutput.AppendText(
                                $"Conflict at offset 0x{i:X8}  Original:{o:X2}  ModA:{a:X2}  ModB:{b:X2}\r\n");
                                continue;
                            }

                            if (mode == MergeMode.AbortOnConflict) //Not merge safe
                            {
                                txtOutput.AppendText("\r\nMerge aborted due to conflict.");
                                return;
                            }
                            else if (mode == MergeMode.ModA) //Overwrite conflict
                            {
                                result = a;
                            }
                            else
                            {
                                result = b;
                            }
                        }
                    }
                    else if (aChanged)  //Results
                    {
                        result = a;
                    }
                    else if (bChanged)
                    {
                        result = b;
                    }
                    else
                    {
                        result = o;
                    }

                    if (!checkOnly) //No conflicts write data
                        outStream.WriteByte(result);
                }

                if (checkOnly)
                {
                    txtOutput.AppendText(conflictFound
                        ? $"\r\nConflicts were detected. Total data conflicts: {conflictCount}"
                        : "\r\nNo conflicts detected.");
                }
                else
                {
                    txtOutput.AppendText(conflictFound && mode != MergeMode.AbortOnConflict
                        ? $"\r\nConflicts resolved using mode: {mode}"
                        : "\r\nMerge completed successfully.");

                    txtOutput.AppendText($"\r\nMerged file saved to: {mergedFile}");
                }
            }
        }
    }
}
