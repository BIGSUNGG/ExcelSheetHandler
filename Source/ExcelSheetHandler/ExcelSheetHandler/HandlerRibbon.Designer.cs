namespace ExcelSheetHandler
{
    partial class HandlerRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public HandlerRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Handler = this.Factory.CreateRibbonTab();
            this.ReadGroup = this.Factory.CreateRibbonGroup();
            this.GenerateClassCodeBtn = this.Factory.CreateRibbonButton();
            this.GenerateJsonBtn = this.Factory.CreateRibbonButton();
            this.ToJsonBtn = this.Factory.CreateRibbonButton();
            this.Handler.SuspendLayout();
            this.ReadGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // Handler
            // 
            this.Handler.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.Handler.Groups.Add(this.ReadGroup);
            this.Handler.Label = "Handler";
            this.Handler.Name = "Handler";
            // 
            // ReadGroup
            // 
            this.ReadGroup.Items.Add(this.GenerateClassCodeBtn);
            this.ReadGroup.Items.Add(this.GenerateJsonBtn);
            this.ReadGroup.Items.Add(this.ToJsonBtn);
            this.ReadGroup.Label = "Read";
            this.ReadGroup.Name = "ReadGroup";
            // 
            // GenerateClassCodeBtn
            // 
            this.GenerateClassCodeBtn.Label = "GenerateClassCode";
            this.GenerateClassCodeBtn.Name = "GenerateClassCodeBtn";
            this.GenerateClassCodeBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.GenerateClassCodeBtn_Click);
            // 
            // GenerateJsonBtn
            // 
            this.GenerateJsonBtn.Label = "GenerateJson";
            this.GenerateJsonBtn.Name = "GenerateJsonBtn";
            this.GenerateJsonBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.GenerateJsonBtn_Click);
            // 
            // HandlerRibbon
            // 
            this.Name = "HandlerRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.Handler);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.HandlerRibbon_Load);
            this.Handler.ResumeLayout(false);
            this.Handler.PerformLayout();
            this.ReadGroup.ResumeLayout(false);
            this.ReadGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab Handler;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup ReadGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ToJsonBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton GenerateClassCodeBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton GenerateJsonBtn;
    }

    partial class ThisRibbonCollection
    {
        internal HandlerRibbon HandlerRibbon
        {
            get { return this.GetRibbon<HandlerRibbon>(); }
        }
    }
}
