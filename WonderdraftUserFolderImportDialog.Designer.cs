namespace MapCreator
{
    partial class WonderdraftUserFolderImportDlg
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
            CollectionListBox = new CheckedListBox();
            ChooseUserFolderButton = new FontAwesome.Sharp.IconButton();
            AddAssetsButton = new FontAwesome.Sharp.IconButton();
            CloseButton = new Button();
            ImportFoldersButton = new FontAwesome.Sharp.IconButton();
            WDUserFolderLabel = new Label();
            CheckAllCheckbox = new CheckBox();
            SuspendLayout();
            // 
            // CollectionListBox
            // 
            CollectionListBox.FormattingEnabled = true;
            CollectionListBox.Location = new Point(78, 111);
            CollectionListBox.Name = "CollectionListBox";
            CollectionListBox.Size = new Size(346, 418);
            CollectionListBox.TabIndex = 0;
            // 
            // ChooseUserFolderButton
            // 
            ChooseUserFolderButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChooseUserFolderButton.IconChar = FontAwesome.Sharp.IconChar.Folder;
            ChooseUserFolderButton.IconColor = Color.Black;
            ChooseUserFolderButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ChooseUserFolderButton.IconSize = 32;
            ChooseUserFolderButton.Location = new Point(12, 21);
            ChooseUserFolderButton.Name = "ChooseUserFolderButton";
            ChooseUserFolderButton.Size = new Size(60, 60);
            ChooseUserFolderButton.TabIndex = 2;
            ChooseUserFolderButton.Text = "Folders";
            ChooseUserFolderButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ChooseUserFolderButton.UseVisualStyleBackColor = true;
            ChooseUserFolderButton.Click += ChooseUserFolderButton_Click;
            ChooseUserFolderButton.MouseHover += ChooseUserFolderButton_MouseHover;
            // 
            // AddAssetsButton
            // 
            AddAssetsButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AddAssetsButton.IconChar = FontAwesome.Sharp.IconChar.FolderPlus;
            AddAssetsButton.IconColor = Color.Black;
            AddAssetsButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            AddAssetsButton.IconSize = 32;
            AddAssetsButton.Location = new Point(12, 157);
            AddAssetsButton.Name = "AddAssetsButton";
            AddAssetsButton.Size = new Size(60, 64);
            AddAssetsButton.TabIndex = 6;
            AddAssetsButton.Text = "Add";
            AddAssetsButton.TextImageRelation = TextImageRelation.TextAboveImage;
            AddAssetsButton.UseVisualStyleBackColor = true;
            AddAssetsButton.Click += AddAssetsButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CloseButton.Location = new Point(364, 539);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(60, 60);
            CloseButton.TabIndex = 7;
            CloseButton.Text = "&Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // ImportFoldersButton
            // 
            ImportFoldersButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ImportFoldersButton.IconChar = FontAwesome.Sharp.IconChar.FileUpload;
            ImportFoldersButton.IconColor = Color.Black;
            ImportFoldersButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ImportFoldersButton.IconSize = 32;
            ImportFoldersButton.Location = new Point(12, 87);
            ImportFoldersButton.Name = "ImportFoldersButton";
            ImportFoldersButton.Size = new Size(60, 64);
            ImportFoldersButton.TabIndex = 8;
            ImportFoldersButton.Text = "Import";
            ImportFoldersButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ImportFoldersButton.UseVisualStyleBackColor = true;
            ImportFoldersButton.Click += ImportFoldersButton_Click;
            // 
            // WDUserFolderLabel
            // 
            WDUserFolderLabel.AutoEllipsis = true;
            WDUserFolderLabel.BorderStyle = BorderStyle.FixedSingle;
            WDUserFolderLabel.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            WDUserFolderLabel.Location = new Point(78, 37);
            WDUserFolderLabel.Name = "WDUserFolderLabel";
            WDUserFolderLabel.Size = new Size(346, 28);
            WDUserFolderLabel.TabIndex = 9;
            WDUserFolderLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CheckAllCheckbox
            // 
            CheckAllCheckbox.AutoSize = true;
            CheckAllCheckbox.Location = new Point(81, 89);
            CheckAllCheckbox.Name = "CheckAllCheckbox";
            CheckAllCheckbox.Size = new Size(130, 20);
            CheckAllCheckbox.TabIndex = 10;
            CheckAllCheckbox.Text = "Check/Uncheck All";
            CheckAllCheckbox.UseVisualStyleBackColor = true;
            CheckAllCheckbox.CheckStateChanged += CheckAllCheckbox_CheckStateChanged;
            // 
            // WonderdraftUserFolderImportDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(436, 611);
            Controls.Add(CheckAllCheckbox);
            Controls.Add(WDUserFolderLabel);
            Controls.Add(ImportFoldersButton);
            Controls.Add(CloseButton);
            Controls.Add(AddAssetsButton);
            Controls.Add(ChooseUserFolderButton);
            Controls.Add(CollectionListBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WonderdraftUserFolderImportDlg";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Wonderdraft User Folder Import";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox CollectionListBox;
        private FontAwesome.Sharp.IconButton ChooseUserFolderButton;
        private FontAwesome.Sharp.IconButton AddAssetsButton;
        private Button CloseButton;
        private FontAwesome.Sharp.IconButton ImportFoldersButton;
        private Label WDUserFolderLabel;
        private CheckBox CheckAllCheckbox;
    }
}