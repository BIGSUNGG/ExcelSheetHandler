using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExcelSheetHandler
{
    /// <summary>
    /// 텍스트를 표시하고 복사할 수 있는 커스텀 다이얼로그
    /// </summary>
    public partial class TextDisplayDialog : Form
    {
        private TextBox textBox;
        private Button copyButton;
        private Button closeButton;

        public TextDisplayDialog(string title, string text)
        {
            InitializeComponent();
            this.Text = title;
            this.textBox.Text = text;
        }

        private void InitializeComponent()
        {
            this.textBox = new TextBox();
            this.copyButton = new Button();
            this.closeButton = new Button();
            this.SuspendLayout();

            // TextBox 설정
            this.textBox.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
            | AnchorStyles.Left) 
            | AnchorStyles.Right)));
            this.textBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.textBox.Location = new Point(12, 12);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = ScrollBars.Both;
            this.textBox.Size = new Size(760, 400);
            this.textBox.TabIndex = 0;

            // Copy 버튼 설정
            this.copyButton.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.copyButton.Location = new Point(616, 425);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new Size(75, 23);
            this.copyButton.TabIndex = 1;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new EventHandler(this.CopyButton_Click);

            // Close 버튼 설정
            this.closeButton.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.closeButton.DialogResult = DialogResult.OK;
            this.closeButton.Location = new Point(697, 425);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;

            // Form 설정
            this.AutoScaleDimensions = new SizeF(7F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(784, 461);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.textBox);
            this.MinimumSize = new Size(600, 400);
            this.Name = "TextDisplayDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(this.textBox.Text);
                MessageBox.Show("텍스트가 클립보드에 복사되었습니다.", "복사 완료", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"복사 중 오류가 발생했습니다: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 텍스트 표시 다이얼로그를 표시합니다
        /// </summary>
        /// <param name="title">다이얼로그 제목</param>
        /// <param name="text">표시할 텍스트</param>
        public static void Show(string title, string text)
        {
            using (var dialog = new TextDisplayDialog(title, text))
            {
                dialog.ShowDialog();
            }
        }
    }
}
