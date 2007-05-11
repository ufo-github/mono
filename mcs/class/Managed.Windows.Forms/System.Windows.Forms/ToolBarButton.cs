// System.Windows.Forms.ToolBarButton.cs
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (C) 2004-2006 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Ravindra (rkumar@novell.com)
//	Mike Kestner <mkestner@novell.com>
//	Everaldo Canuto <ecanuto@novell.com>

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
	[DefaultProperty ("Text")]
	[Designer ("System.Windows.Forms.Design.ToolBarButtonDesigner, " + Consts.AssemblySystem_Design, "System.ComponentModel.Design.IDesigner")]
	[DesignTimeVisible (false)]
	[ToolboxItem (false)]
	public class ToolBarButton : Component
	{
		#region instance variable
		private bool enabled = true;
		private int image_index = -1;
		private ContextMenu menu;
		private ToolBar parent;
		private bool partial_push = false;
		private bool pushed = false;
		private ToolBarButtonStyle style = ToolBarButtonStyle.PushButton;
		private object tag;
		private string text = "";
		private string tooltip = "";
		private bool visible = true;
		#endregion

		#region constructors
		
		public ToolBarButton () { }

		public ToolBarButton (string text)
		{
			this.text = text;
		}
		
		#endregion

		#region internal properties

		internal Image Image {
			get {
				if (Parent == null || Parent.ImageList == null)
					return null;

				ImageList list = Parent.ImageList;
				if (ImageIndex > -1 && ImageIndex < list.Images.Count)
					return list.Images [ImageIndex];

				return null;
			}
		}

		#endregion internal properties

		#region properties

		[DefaultValue (null)]
		[TypeConverter (typeof (ReferenceConverter))]
		public Menu DropDownMenu {
			get { return menu; }

			set {
				if (value is ContextMenu)
					menu = (ContextMenu) value;
				else
					throw new ArgumentException ("DropDownMenu must be of type ContextMenu.");
			}
		}

		[DefaultValue (true)]
		[Localizable (true)]
		public bool Enabled {
			get { return enabled; }
			set {
				if (value == enabled)
					return;

				enabled = value;
				Invalidate ();
			}
		}

#if NET_2_0
		[RefreshProperties (RefreshProperties.Repaint)]
#endif
		[DefaultValue (-1)]
		[Editor ("System.Windows.Forms.Design.ImageIndexEditor, " + Consts.AssemblySystem_Design, typeof (System.Drawing.Design.UITypeEditor))]
		[Localizable (true)]
		[TypeConverter (typeof (ImageIndexConverter))]
		public int ImageIndex {
			get { return image_index; }
			set {
				if (value < -1)
					throw new ArgumentException ("ImageIndex value must be above or equal to -1.");

				if (value == image_index)
					return;

				bool layout = (Parent != null) && ((value == -1) || (image_index == -1));
				
				image_index = value;
				
				if (layout)
					Parent.Redraw (true);
				else
					Invalidate ();
			}
		}

		[Browsable (false)]
		public ToolBar Parent {
			get { return parent; }
		}

		[DefaultValue (false)]
		public bool PartialPush {
			get { return partial_push; }
			set {
				if (value == partial_push)
					return;

				partial_push = value;
				Invalidate ();
			}
		}

		[DefaultValue (false)]
		public bool Pushed {
			get { return pushed; }
			set {
				if (value == pushed)
					return;

				pushed = value;
				Invalidate ();
			}
		}

		public Rectangle Rectangle {
			get {
				if (Visible && Parent != null && Parent.items != null)
					foreach (ToolBarItem item in Parent.items)
						if (item.Button == this)
							return item.Rectangle;
					
				return Rectangle.Empty;
			}
		}

		[DefaultValue (ToolBarButtonStyle.PushButton)]
		[RefreshProperties (RefreshProperties.Repaint)]
		public ToolBarButtonStyle Style {
			get { return style; }
			set {
				if (value == style)
					return;

				style = value;
				
				if (parent != null)
					parent.Redraw (true);
			}
		}

		[Bindable (true)]
		[DefaultValue (null)]
		[Localizable (false)]
		[TypeConverter (typeof (StringConverter))]
		public object Tag {
			get { return tag; }
			set { tag = value; }
		}

		[DefaultValue ("")]
		[Localizable (true)]
		public string Text {
			get { return text; }
			set {
				if (value == null) value = "";

				if (value == text)
					return;

				text = value;
				if (Parent != null)
					Parent.Redraw (true);
			}
		}

		[DefaultValue ("")]
		[Localizable (true)]
		public string ToolTipText {
			get { return tooltip; }
			set {
				if (value == null) value = "";
				tooltip = value;
			}
		}

		[DefaultValue (true)]
		[Localizable (true)]
		public bool Visible {
			get { return visible; }
			set {
				if (value == visible)
					return;

				visible = value;
				if (Parent != null)
					Parent.Redraw (true);
			}
		}

		#endregion

		#region internal methods

		internal void SetParent (ToolBar parent)
		{
			if (Parent == parent)
				return;

			if (Parent != null)
				Parent.Buttons.Remove (this);

			this.parent = parent;
		}

		internal void Invalidate ()
		{
			if (Parent != null)
				Parent.Invalidate (Rectangle);
		}

		#endregion Internal Methods

		#region methods

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}

		public override string ToString ()
		{
			return string.Format ("ToolBarButton: {0}, Style: {1}", text, style);
		}
		
		#endregion
	}
}
