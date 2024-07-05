namespace MapCreator
{
    partial class WonderdraftAssetImportDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ChooseZipFileButton = new FontAwesome.Sharp.IconButton();
            ZipFilePathLabel = new Label();
            ImportButton = new FontAwesome.Sharp.IconButton();
            FilePreviewTree = new TreeView();
            CloseButton = new Button();
            AddCollectionButton = new FontAwesome.Sharp.IconButton();
            SuspendLayout();
            // 
            // ChooseZipFileButton
            // 
            ChooseZipFileButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChooseZipFileButton.IconChar = FontAwesome.Sharp.IconChar.FileZipper;
            ChooseZipFileButton.IconColor = Color.Black;
            ChooseZipFileButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ChooseZipFileButton.IconSize = 32;
            ChooseZipFileButton.Location = new Point(31, 37);
            ChooseZipFileButton.Name = "ChooseZipFileButton";
            ChooseZipFileButton.Size = new Size(60, 64);
            ChooseZipFileButton.TabIndex = 0;
            ChooseZipFileButton.Text = "Zip File";
            ChooseZipFileButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ChooseZipFileButton.UseVisualStyleBackColor = true;
            ChooseZipFileButton.Click += ChooseZipFileButton_Click;
            // 
            // ZipFilePathLabel
            // 
            ZipFilePathLabel.BorderStyle = BorderStyle.FixedSingle;
            ZipFilePathLabel.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ZipFilePathLabel.Location = new Point(117, 54);
            ZipFilePathLabel.Name = "ZipFilePathLabel";
            ZipFilePathLabel.Size = new Size(671, 31);
            ZipFilePathLabel.TabIndex = 1;
            ZipFilePathLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ImportButton
            // 
            ImportButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ImportButton.IconChar = FontAwesome.Sharp.IconChar.FileUpload;
            ImportButton.IconColor = Color.Black;
            ImportButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ImportButton.IconSize = 32;
            ImportButton.Location = new Point(31, 107);
            ImportButton.Name = "ImportButton";
            ImportButton.Size = new Size(60, 64);
            ImportButton.TabIndex = 2;
            ImportButton.Text = "Import";
            ImportButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ImportButton.UseVisualStyleBackColor = true;
            ImportButton.Click += ImportButton_Click;
            // 
            // FilePreviewTree
            // 
            FilePreviewTree.CheckBoxes = true;
            FilePreviewTree.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FilePreviewTree.Location = new Point(117, 108);
            FilePreviewTree.Name = "FilePreviewTree";
            FilePreviewTree.Size = new Size(671, 230);
            FilePreviewTree.TabIndex = 3;
            // 
            // CloseButton
            // 
            CloseButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CloseButton.Location = new Point(728, 408);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(60, 60);
            CloseButton.TabIndex = 4;
            CloseButton.Text = "&Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // AddCollectionButton
            // 
            AddCollectionButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AddCollectionButton.IconChar = FontAwesome.Sharp.IconChar.FolderPlus;
            AddCollectionButton.IconColor = Color.Black;
            AddCollectionButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            AddCollectionButton.IconSize = 32;
            AddCollectionButton.Location = new Point(31, 177);
            AddCollectionButton.Name = "AddCollectionButton";
            AddCollectionButton.Size = new Size(60, 64);
            AddCollectionButton.TabIndex = 5;
            AddCollectionButton.Text = "Add";
            AddCollectionButton.TextImageRelation = TextImageRelation.TextAboveImage;
            AddCollectionButton.UseVisualStyleBackColor = true;
            AddCollectionButton.Click += AddCollectionButton_Click;
            // 
            // WonderdraftAssetImportDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(800, 480);
            Controls.Add(AddCollectionButton);
            Controls.Add(CloseButton);
            Controls.Add(FilePreviewTree);
            Controls.Add(ImportButton);
            Controls.Add(ZipFilePathLabel);
            Controls.Add(ChooseZipFileButton);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WonderdraftAssetImportDialog";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import Wonderdraft Assets";
            ResumeLayout(false);
        }

        #endregion

        private FontAwesome.Sharp.IconButton ChooseZipFileButton;
        private Label ZipFilePathLabel;
        private FontAwesome.Sharp.IconButton ImportButton;
        private TreeView FilePreviewTree;
        private Button CloseButton;
        private FontAwesome.Sharp.IconButton AddCollectionButton;
    }
}