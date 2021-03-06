using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DrawingPanel;
namespace TriadNSim.Forms
{
    public partial class frmObjectIPs : Form
    {
        private NetworkObject _obj;

        public frmObjectIPs(NetworkObject Obj)
        {
            InitializeComponent();
            _obj = Obj;
            Text = _obj.Name + " - " + Text;
        }

        private void frmObjectIPs_Load(object sender, EventArgs e)
        {
            foreach (ConnectedIP ip in _obj.ConnectedIPs)
                AddIp(ip);
            AdjustColumnsWidth();
            if (lvIP.Items.Count > 0)
                lvIP.Items[0].Selected = true;
        }

        private void AdjustColumnsWidth()
        {
            ColumnHeaderAutoResizeStyle style = lvIP.Items.Count > 0 ? ColumnHeaderAutoResizeStyle.ColumnContent :
                                                                        ColumnHeaderAutoResizeStyle.HeaderSize;
            lvIP.AutoResizeColumn(0, style);
            lvIP.AutoResizeColumn(1, style);
            this.chDescription.Width = lvIP.Width - this.chName.Width - this.chType.Width - 4;
        }

        private void AddIp(ConnectedIP ip)
        {
            InsertIp(lvIP.Items.Count, ip);
        }

        private void InsertIp(int iIndex, ConnectedIP ip)
        {
            string sName = ip.IP.Name+"(";
            for (int i = 0; i < ip.Params.Count; i++)
            {
                if (i > 0)
                    sName += ",";
                sName += ip.Params[i];
                sName += ")";
            }
            ListViewItem item = new ListViewItem(sName);
            item.SubItems.Add(ip.IP.IsStandart ? "Стандартная" : "Пользовательская");
            item.SubItems.Add(ip.Description.Length > 0 ? ip.Description : ip.IP.Description);
            lvIP.Items.Insert(iIndex, item);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmObjectIP frmObjectIP = new frmObjectIP(_obj);
            if (frmObjectIP.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (_obj.ConnectedIPs == null)
                    _obj.ConnectedIPs = new List<ConnectedIP>();
                _obj.ConnectedIPs.Add(frmObjectIP.Result);
                AddIp(frmObjectIP.Result);
                AdjustColumnsWidth();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int iIndex = lvIP.SelectedIndices[0];
            if (Util.ShowConformationBox("Удалить ИП " + lvIP.Items[iIndex].Text + "?"))
            {
                _obj.ConnectedIPs.RemoveAt(iIndex);
                lvIP.Items.RemoveAt(iIndex);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int iIndex = lvIP.SelectedIndices[0];
            frmObjectIP frmObjectIP = new frmObjectIP(_obj);
            frmObjectIP.SelectIP(_obj.ConnectedIPs[iIndex]);
            if (frmObjectIP.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _obj.ConnectedIPs[iIndex] = frmObjectIP.Result;
                lvIP.Items.RemoveAt(iIndex); 
                InsertIp(iIndex, frmObjectIP.Result);
                AdjustColumnsWidth();
            }
        }

        private void lvIP_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnDelete.Enabled = lvIP.SelectedItems.Count > 0;
        }
    }
}
