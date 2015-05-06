using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using GrinGlobal.Core;

namespace GrinGlobal.Admin {
    public class GuiManager {

        private GuiManager() { }

        public static void LoadResources(DataTable dt, string appName, Form f, List<Component> components){

            if (dt != null) {

                var drs = dt.Select("app_name = '" + appName + "' and (form_name = '" + f.Name + "' or form_name = 'all')");

                if (drs != null && drs.Length > 0) {

                    // drs now contains all rows specific to the given form.
                    // throw those into a dictionary so they're easy to pluck out when we spin through the form controls
                    var dic = new Dictionary<string, string>();
                    foreach (DataRow dr in drs) {
                        dic[dr["app_resource_name"].ToString()] = dr["display_member"].ToString();
                        dic[dr["app_resource_name"].ToString() + "_value"] = dr["value_member"].ToString();
                        dic[dr["app_resource_name"].ToString() + "_title"] = dr["description"].ToString();
                    }


                    // form itself may need its name munged
                    string val = null;
                    if (dic.TryGetValue(f.Name, out val)) {
                        initComponent(f, val);
                    }

                    // spin through all controls recursively and initialize as needed
                    foreach (Control c in TraverseDescendentControls(f)) {
                        val = null;
                        if (dic.TryGetValue(c.Name, out val)) {
                            initComponent(c, val);
                        }
                        if (c is ListView) {
                            foreach (ColumnHeader ch in ((ListView)c).Columns) {
                                if (dic.TryGetValue(ch.Name, out val)) {
                                    initComponent(ch, val);
                                }
                            }
                        } else if (c is DataGridView) {
                            foreach (DataGridViewColumn col in ((DataGridView)c).Columns) {
                                if (dic.TryGetValue(col.Name, out val)) {
                                    initComponent(col, val);
                                }
                            }
                        }
                    }


                    foreach (Component co in components) {
                        var cm = co as ContextMenuStrip;
                        if (cm != null){
                            loadMenuItems(cm, dic);
                        }
                        var ms = co as MenuStrip;
                        if (ms != null) {
                            loadMenuItems(ms, dic);
                        }
                    }
                }
            }
        }

        private static void loadMenuItems(ContextMenuStrip cms, Dictionary<string, string> dic) {
            string val = null;
            foreach (ToolStripItem tsi in cms.Items) {
                if (dic.TryGetValue(tsi.Name, out val)){
                    tsi.Text = val;
                }
            }
        }

        private static void loadMenuItems(MenuStrip ms, Dictionary<string, string> dic) {
            string val = null;
            foreach (ToolStripItem tsi in ms.Items) {
                if (dic.TryGetValue(tsi.Name, out val)) {
                    tsi.Text = val;
                }
                var tsddi = tsi as ToolStripDropDownItem;
                if (tsddi != null) {
                    if (tsddi.DropDownItems.Count > 0) {
                        loadMenuItems(tsddi, dic);
                    }
                }
            }
        }

        private static void loadMenuItems(ToolStripDropDownItem ddi, Dictionary<string, string> dic) {
            string val = null;
            foreach (ToolStripItem tsi in ddi.DropDownItems) {
                if (dic.TryGetValue(tsi.Name, out val)) {
                    tsi.Text = val;
                }
                var tsddi = tsi as ToolStripDropDownItem;
                if (tsddi != null) {
                    if (tsddi.DropDownItems.Count > 0) {
                        loadMenuItems(tsddi, dic);
                    }
                }
            }
        }

        private static void initComponent(Component c, string value){
            if (c is Control){
                // just use the value as the text value
                ((Control)c).Text = value;
            } else if (c is ColumnHeader) {
                // list view header
                ((ColumnHeader)c).Text = value;
            }
        }

        private static void initComponent(ColumnHeader c, string value) {
            c.Text = value;
        }

        private static void initComponent(DataGridViewColumn dgvc, string value) {
            dgvc.HeaderText = value;
        }


        public static IEnumerable<Control> TraverseDescendentControls(Control parent) {
            foreach (Control c in parent.Controls) {
                yield return c;
                foreach (Control c2 in TraverseDescendentControls(c)) {
                    yield return c2;
                }
            }
        }

        private static Dictionary<TreeView, long> __treeViewTicks;
        public static TreeNode TreeViewSyncDrag(TreeView tv, DragEventArgs e) {
            if (__treeViewTicks == null) {
                __treeViewTicks = new Dictionary<TreeView, long>();
            }
            long prevTicks;
            if (!__treeViewTicks.TryGetValue(tv, out prevTicks)) {
                prevTicks = DateTime.Now.Ticks;
            }

            e.Effect = DragDropEffects.None;

            var pt = tv.PointToClient(new Point(e.X, e.Y));
            var nd = tv.GetNodeAt(pt);

            var nowTicks = DateTime.Now.Ticks;
            __treeViewTicks[tv] = nowTicks;
            var duration = new TimeSpan(nowTicks - prevTicks).TotalMilliseconds;

            if (nd != null) {

                if (pt.X < 16) {
                    nd.EnsureVisible();
                }

                if (pt.Y < tv.ItemHeight) {
                    if (nd.PrevVisibleNode != null) {
                        //if (nd.PrevVisibleNode.Tag is ConnectionInfo) {
                        //    return nd;
                        //}
                        var prev = nd.PrevVisibleNode;
                        if (!prev.IsExpanded) {
                            prev.Expand();
                            if (prev.Nodes.Count > 0) {
                                nd = prev.Nodes[prev.Nodes.Count - 1];
                            } else {
                                nd = prev;
                            }
                        } else {
                            nd = nd.PrevVisibleNode;
                        }
                    }
                    nd.EnsureVisible();
                } else if (pt.Y < (tv.ItemHeight * 2)) {
                    if (duration > 250) {
                        //if (nd.PrevVisibleNode.Tag is ConnectionInfo) {
                        //    return nd;
                        //}
                        nd = nd.PrevVisibleNode;
                        if (nd.PrevVisibleNode != null) {
                            var prev = nd.PrevVisibleNode;
                            if (!prev.IsExpanded) {
                                prev.Expand();
                                if (prev.Nodes.Count > 0) {
                                    nd = prev.Nodes[prev.Nodes.Count - 1];
                                } else {
                                    nd = prev;
                                }
                            } else {
                                nd = nd.PrevVisibleNode;
                            }
                        }
                        nd.EnsureVisible();
                    }
                } else if (pt.Y > (tv.Height - tv.ItemHeight * 2)) {
                    Debug.WriteLine("fast scroll down");
                    //if (nd.NextVisibleNode.Tag is ConnectionInfo) {
                    //    return nd;
                    //}
                    nd = nd.NextVisibleNode;
                    if (nd.NextVisibleNode != null) {
                        var next = nd.NextVisibleNode;
                        if (!next.IsExpanded) {
                            next.Expand();
                            nd = next;
                        } else {
                            nd = nd.NextVisibleNode;
                        }
                    }
                    nd.EnsureVisible();
                } else if (pt.Y > tv.Height - tv.ItemHeight * 3) {
                    Debug.WriteLine("slow scroll down");
                    if (duration > 250) {
                        if (nd.NextVisibleNode != null) {
                            //if (nd.NextVisibleNode.Tag is ConnectionInfo) {
                            //    return nd;
                            //}
                            var next = nd.NextVisibleNode;
                            if (!next.IsExpanded) {
                                next.Expand();
                                nd = next;
                            } else {
                                nd = nd.NextVisibleNode;
                            }
                        }
                        nd.EnsureVisible();
                    }
                }
            }

            return nd;
        }


    }
}
