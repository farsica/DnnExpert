//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2010
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System.Web;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

using DotNetNuke.Security;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;

using Telerik.Web.UI;
using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Web.UI.WebControls;

namespace DotNetNuke.UI.Skins.Controls
{

    public partial class DNNRadMenu : UI.Skins.SkinObjectBase
    {

        #region Private Variables

        //variables and structures
        private Queue PagesQueue;
        private ArrayList AuthPages;
        private ArrayList arrayShowPath;
        private string dnnSkinSrc = "";
        private string dnnSkinPath = "";

        private struct qElement
        {
            public RadMenuItem radMenuItem;
            public TabInfo page;
            public int item;
        }


        private string _Style = "";
        //menu group properties
        private ItemFlow _GroupFlow = ItemFlow.Vertical;
        private ExpandDirection _GroupExpandDirection = ExpandDirection.Auto;
        private int _GroupOffsetX = 0;
        private int _GroupOffsetY = 0;
        private Unit _GroupWidth = Unit.Empty;

        private Unit _GroupHeight = Unit.Empty;
        //menu item properties
        private string _ItemCssClass = "";
        private string _ItemDisabledCssClass = "";
        private string _ItemExpandedCssClass = "";
        private string _ItemFocusedCssClass = "";
        private string _ItemClickedCssClass = "";
        private string _ItemImageUrl = "";
        private string _ItemHoveredImageUrl = "";
        private string _ItemTarget = "";
        private Color _ItemBackColor = Color.Empty;
        private Color _ItemBorderColor = Color.Empty;
        private Unit _ItemBorderWidth = Unit.Empty;
        private BorderStyle _ItemBorderStyle = BorderStyle.None;
        private Color _ItemForeColor = Color.Empty;
        private Unit _ItemHeight = Unit.Empty;

        private Unit _ItemWidth = Unit.Empty;
        //menu ROOT item properties
        private string _RootItemCssClass = "";
        private string _RootItemDisabledCssClass = "";
        private string _RootItemExpandedCssClass = "";
        private string _RootItemFocusedCssClass = "";
        private string _RootItemClickedCssClass = "";
        private string _RootItemImageUrl = "";
        private string _RootItemHoveredImageUrl = "";
        private string _RootItemTarget = "";
        private Color _RootItemBackColor = Color.Empty;
        private Color _RootItemBorderColor = Color.Empty;
        private Unit _RootItemBorderWidth = Unit.Empty;
        private BorderStyle _RootItemBorderStyle = BorderStyle.None;
        private Color _RootItemForeColor = Color.Empty;
        private Unit _RootItemHeight = Unit.Empty;

        private Unit _RootItemWidth = Unit.Empty;
        //separator and first/last items
        private string _HeaderFirstItem;
        private string _HeaderFirstItemCssClass;
        private string _HeaderFirstItemCssClassClicked;

        private string _HeaderFirstItemCssClassOver;
        private string _HeaderSeparator;
        private string _HeaderSeparatorCssClass;
        private string _HeaderSeparatorCssClassClicked;

        private string _HeaderSeparatorCssClassOver;
        private string _HeaderLastItem;
        private string _HeaderLastItemCssClass;
        private string _HeaderLastItemCssClassClicked;

        private string _HeaderLastItemCssClassOver;
        private string _ChildGroupFirstItem;
        private string _ChildGroupFirstItemCssClass;
        private string _ChildGroupFirstItemCssClassClicked;

        private string _ChildGroupFirstItemCssClassOver;
        private string _ChildGroupSeparator;
        private string _ChildGroupSeparatorCssClass;
        private string _ChildGroupSeparatorCssClassClicked;

        private string _ChildGroupSeparatorCssClassOver;
        private string _ChildGroupLastItem;
        private string _ChildGroupLastItemCssClass;
        private string _ChildGroupLastItemCssClassClicked;

        private string _ChildGroupLastItemCssClassOver;
        private string _SelectedPathHeaderItemCss;
        private string _SelectedPathItemCss;
        private string _SelectedPathItemImage;

        private string _SelectedPathHeaderItemImage;
        //other properties
        private bool _ShowPath = false;
        private bool _EnableToolTips;
        private bool _ImagesOnlyMenu = false;
        private bool _EnableLevelCss = false;
        private bool _EnableItemCss = false;
        private bool _EnableRootItemCss = false;
        private int _MaxLevelNumber = 10;
        private int _MaxItemNumber = 20;
        private int _MaxLevel = -1;
        private string _ShowOnlyCurrent = "";
        private bool _EnablePageIdCssClass = false;
        private bool _EnablePageIcons = true;
        private bool _EnableUserMenus = true;
        private bool _EnableAdminMenus = true;
        private bool _CopyChildItemLink = false;
        private string _PagesToExclude = "";
        private bool _UseTitle = false;

        private bool _ContainHostTabs = false;
        private DnnMenu _radMenu = null;
        protected DnnMenu RadMenu1
        {
            get
            {
                if (((_radMenu == null)))
                {
                    _radMenu = new DnnMenu();
                    _radMenu.ID = "psMenu1";
                    psMenu.Controls.Add(_radMenu);
                }
                return _radMenu;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public RadMenuItem FindItem(System.Predicate<RadMenuItem> match)
        {
            return RadMenu1.FindItem(match);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public RadMenuItem FindItemByText(string text)
        {
            return RadMenu1.FindItemByText(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public RadMenuItem FindItemByText(string text, bool ignoreCase)
        {
            return RadMenu1.FindItemByText(text, ignoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public RadMenuItem FindItemByUrl(string url)
        {
            return RadMenu1.FindItemByUrl(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public RadMenuItemCollection Items()
        {
            return RadMenu1.Items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Collections.Generic.IList<RadMenuItem> GetAllItems()
        {
            return RadMenu1.GetAllItems();
        }

        #endregion

        #region Public Properties

        public bool UseTitle
        {
            get { return _UseTitle; }
            set { _UseTitle = value; }
        }

        public bool ContainHostTabs
        {
            get { return _ContainHostTabs; }
            set { _ContainHostTabs = value; }
        }

        public int AutoScrollMinimumHeight
        {
            get { return RadMenu1.AutoScrollMinimumHeight; }
            set { RadMenu1.AutoScrollMinimumHeight = value; }
        }

        public int AutoScrollMinimumWidth
        {
            get { return RadMenu1.AutoScrollMinimumWidth; }
            set { RadMenu1.AutoScrollMinimumWidth = value; }
        }

        public AnimationType ExpandAnimationType
        {
            get { return RadMenu1.ExpandAnimation.Type; }
            set { RadMenu1.ExpandAnimation.Type = value; }
        }

        public int ExpandAnimationDuration
        {
            get { return RadMenu1.ExpandAnimation.Duration; }
            set { RadMenu1.ExpandAnimation.Duration = value; }
        }

        public bool CausesValidation
        {
            get { return RadMenu1.CausesValidation; }
            set { RadMenu1.CausesValidation = value; }
        }

        public string ChildGroupFirstItem
        {
            get { return _ChildGroupFirstItem; }
            set { _ChildGroupFirstItem = value; }
        }

        public string ChildGroupFirstItemCssClass
        {
            get { return _ChildGroupFirstItemCssClass; }
            set { _ChildGroupFirstItemCssClass = value; }
        }

        public string ChildGroupFirstItemCssClassClicked
        {
            get { return _ChildGroupFirstItemCssClassClicked; }
            set { _ChildGroupFirstItemCssClassClicked = value; }
        }

        public string ChildGroupFirstItemCssClassOver
        {
            get { return _ChildGroupFirstItemCssClassOver; }
            set { _ChildGroupFirstItemCssClassOver = value; }
        }

        public string ChildGroupSeparator
        {
            get { return _ChildGroupSeparator; }
            set { _ChildGroupSeparator = value; }
        }

        public string ChildGroupSeparatorCssClass
        {
            get { return _ChildGroupSeparatorCssClass; }
            set { _ChildGroupSeparatorCssClass = value; }
        }

        public string ChildGroupSeparatorCssClassClicked
        {
            get { return _ChildGroupSeparatorCssClassClicked; }
            set { _ChildGroupSeparatorCssClassClicked = value; }
        }

        public string ChildGroupSeparatorCssClassOver
        {
            get { return _ChildGroupSeparatorCssClassOver; }
            set { _ChildGroupSeparatorCssClassOver = value; }
        }

        public string ChildGroupLastItem
        {
            get { return _ChildGroupLastItem; }
            set { _ChildGroupLastItem = value; }
        }

        public string ChildGroupLastItemCssClass
        {
            get { return _ChildGroupLastItemCssClass; }
            set { _ChildGroupLastItemCssClass = value; }
        }

        public string ChildGroupLastItemCssClassClicked
        {
            get { return _ChildGroupLastItemCssClassClicked; }
            set { _ChildGroupLastItemCssClassClicked = value; }
        }

        public string ChildGroupLastItemCssClassOver
        {
            get { return _ChildGroupLastItemCssClassOver; }
            set { _ChildGroupLastItemCssClassOver = value; }
        }

        public AnimationType CollapseAnimationType
        {
            get { return RadMenu1.CollapseAnimation.Type; }
            set { RadMenu1.CollapseAnimation.Type = value; }
        }

        public int CollapseAnimationDuration
        {
            get { return RadMenu1.CollapseAnimation.Duration; }
            set { RadMenu1.CollapseAnimation.Duration = value; }
        }

        public int CollapseDelay
        {
            get { return RadMenu1.CollapseDelay; }
            set { RadMenu1.CollapseDelay = value; }
        }

        public bool ClickToOpen
        {
            get { return RadMenu1.ClickToOpen; }
            set { RadMenu1.ClickToOpen = value; }
        }

        public bool CopyChildItemLink
        {
            get { return _CopyChildItemLink; }
            set { _CopyChildItemLink = value; }
        }

        public string CssClass
        {
            get { return RadMenu1.CssClass; }
            set { RadMenu1.CssClass = value; }
        }

        public string Dir
        {
            get { return RadMenu1.Attributes["dir"]; }
            set { RadMenu1.Attributes["dir"] = value; }
        }

        public bool EnableAdminMenus
        {
            get { return _EnableAdminMenus; }
            set { _EnableAdminMenus = value; }
        }

        public bool EnableAutoScroll
        {
            get { return RadMenu1.EnableAutoScroll; }
            set { RadMenu1.EnableAutoScroll = value; }
        }

        public bool EnableEmbeddedBaseStylesheet
        {
            get { return RadMenu1.EnableEmbeddedBaseStylesheet; }
            set { RadMenu1.EnableEmbeddedBaseStylesheet = value; }
        }

        public bool EnableEmbeddedScripts
        {
            get { return RadMenu1.EnableEmbeddedScripts; }
            set { RadMenu1.EnableEmbeddedScripts = value; }
        }

        public bool EnableEmbeddedSkins
        {
            get { return RadMenu1.EnableEmbeddedSkins; }
            set { RadMenu1.EnableEmbeddedSkins = value; }
        }

        public bool EnableItemCss
        {
            get { return _EnableItemCss; }
            set { _EnableItemCss = value; }
        }

        public bool EnableLevelCss
        {
            get { return _EnableLevelCss; }
            set { _EnableLevelCss = value; }
        }

        public bool EnablePageIcons
        {
            get { return _EnablePageIcons; }
            set { _EnablePageIcons = value; }
        }

        public bool EnablePageIdCssClass
        {
            get { return _EnablePageIdCssClass; }
            set { _EnablePageIdCssClass = value; }
        }

        public bool EnableRootItemCss
        {
            get { return _EnableRootItemCss; }
            set { _EnableRootItemCss = value; }
        }

        public bool EnableScreenBoundaryDetection
        {
            get { return RadMenu1.EnableScreenBoundaryDetection; }
            set { RadMenu1.EnableScreenBoundaryDetection = value; }
        }

        public bool EnableToolTips
        {
            get { return _EnableToolTips; }
            set { _EnableToolTips = value; }
        }

        public bool EnableUserMenus
        {
            get { return _EnableUserMenus; }
            set { _EnableUserMenus = value; }
        }

        public int ExpandDelay
        {
            get { return RadMenu1.ExpandDelay; }
            set { RadMenu1.ExpandDelay = value; }
        }

        public ItemFlow Flow
        {
            get { return RadMenu1.Flow; }
            set { RadMenu1.Flow = value; }
        }

        public ExpandDirection GroupExpandDirection
        {
            get { return _GroupExpandDirection; }
            set { _GroupExpandDirection = value; }
        }

        public ItemFlow GroupFlow
        {
            get { return _GroupFlow; }
            set { _GroupFlow = value; }
        }

        public Unit GroupHeight
        {
            get { return _GroupHeight; }
            set { _GroupHeight = value; }
        }

        public int GroupOffsetX
        {
            get { return _GroupOffsetX; }
            set { _GroupOffsetX = value; }
        }

        public int GroupOffsetY
        {
            get { return _GroupOffsetY; }
            set { _GroupOffsetY = value; }
        }

        public Unit GroupWidth
        {
            get { return _GroupWidth; }
            set { _GroupWidth = value; }
        }

        public Unit Height
        {
            get { return RadMenu1.Height; }
            set { RadMenu1.Height = value; }
        }

        public bool ImagesOnlyMenu
        {
            get { return _ImagesOnlyMenu; }
            set { _ImagesOnlyMenu = value; }
        }

        public Color ItemBackColor
        {
            get { return _ItemBackColor; }
            set { _ItemBackColor = value; }
        }

        public Color ItemBorderColor
        {
            get { return _ItemBorderColor; }
            set { _ItemBorderColor = value; }
        }

        public BorderStyle ItemBorderStyle
        {
            get { return _ItemBorderStyle; }
            set { _ItemBorderStyle = value; }
        }

        public Unit ItemBorderWidth
        {
            get { return _ItemBorderWidth; }
            set { _ItemBorderWidth = value; }
        }

        public string ItemClickedCssClass
        {
            get { return _ItemClickedCssClass; }
            set { _ItemClickedCssClass = value; }
        }

        public string ItemCssClass
        {
            get { return _ItemCssClass; }
            set { _ItemCssClass = value; }
        }

        public string ItemDisabledCssClass
        {
            get { return _ItemDisabledCssClass; }
            set { _ItemDisabledCssClass = value; }
        }

        public string ItemExpandedCssClass
        {
            get { return _ItemExpandedCssClass; }
            set { _ItemExpandedCssClass = value; }
        }

        public string ItemFocusedCssClass
        {
            get { return _ItemFocusedCssClass; }
            set { _ItemFocusedCssClass = value; }
        }

        public Color ItemForeColor
        {
            get { return _ItemForeColor; }
            set { _ItemForeColor = value; }
        }

        public Unit ItemHeight
        {
            get { return _ItemHeight; }
            set { _ItemHeight = value; }
        }

        public string ItemHoveredImageUrl
        {
            get { return _ItemHoveredImageUrl; }
            set { _ItemHoveredImageUrl = value; }
        }

        public string ItemImageUrl
        {
            get { return _ItemImageUrl; }
            set { _ItemImageUrl = value; }
        }

        public string ItemTarget
        {
            get { return _ItemTarget; }
            set { _ItemTarget = value; }
        }

        public Unit ItemWidth
        {
            get { return _ItemWidth; }
            set { _ItemWidth = value; }
        }

        public string HeaderFirstItem
        {
            get { return _HeaderFirstItem; }
            set { _HeaderFirstItem = value; }
        }

        public string HeaderFirstItemCssClass
        {
            get { return _HeaderFirstItemCssClass; }
            set { _HeaderFirstItemCssClass = value; }
        }

        public string HeaderFirstItemCssClassClicked
        {
            get { return _HeaderFirstItemCssClassClicked; }
            set { _HeaderFirstItemCssClassClicked = value; }
        }

        public string HeaderFirstItemCssClassOver
        {
            get { return _HeaderFirstItemCssClassOver; }
            set { _HeaderFirstItemCssClassOver = value; }
        }

        public string HeaderLastItem
        {
            get { return _HeaderLastItem; }
            set { _HeaderLastItem = value; }
        }

        public string HeaderLastItemCssClass
        {
            get { return _HeaderLastItemCssClass; }
            set { _HeaderLastItemCssClass = value; }
        }

        public string HeaderLastItemCssClassClicked
        {
            get { return _HeaderLastItemCssClassClicked; }
            set { _HeaderLastItemCssClassClicked = value; }
        }

        public string HeaderLastItemCssClassOver
        {
            get { return _HeaderLastItemCssClassOver; }
            set { _HeaderLastItemCssClassOver = value; }
        }

        public string HeaderSeparator
        {
            get { return _HeaderSeparator; }
            set { _HeaderSeparator = value; }
        }

        public string HeaderSeparatorCssClass
        {
            get { return _HeaderSeparatorCssClass; }
            set { _HeaderSeparatorCssClass = value; }
        }

        public string HeaderSeparatorCssClassClicked
        {
            get { return _HeaderSeparatorCssClassClicked; }
            set { _HeaderSeparatorCssClassClicked = value; }
        }

        public string HeaderSeparatorCssClassOver
        {
            get { return _HeaderSeparatorCssClassOver; }
            set { _HeaderSeparatorCssClassOver = value; }
        }

        public int MaxItemNumber
        {
            get { return _MaxItemNumber; }
            set { _MaxItemNumber = value; }
        }

        public int MaxLevel
        {
            get { return _MaxLevel; }
            set { _MaxLevel = value; }
        }

        public int MaxLevelNumber
        {
            get { return _MaxLevelNumber; }
            set { _MaxLevelNumber = value; }
        }

        public string OnClientMouseOver
        {
            get { return RadMenu1.OnClientMouseOver; }
            set { RadMenu1.OnClientMouseOver = value; }
        }

        public string OnClientMouseOut
        {
            get { return RadMenu1.OnClientMouseOut; }
            set { RadMenu1.OnClientMouseOut = value; }
        }

        public string OnClientItemFocus
        {
            get { return RadMenu1.OnClientItemFocus; }
            set { RadMenu1.OnClientItemFocus = value; }
        }

        public string OnClientItemBlur
        {
            get { return RadMenu1.OnClientItemBlur; }
            set { RadMenu1.OnClientItemBlur = value; }
        }

        public string OnClientItemClicking
        {
            get { return RadMenu1.OnClientItemClicking; }
            set { RadMenu1.OnClientItemClicking = value; }
        }

        public string OnClientItemClicked
        {
            get { return RadMenu1.OnClientItemClicked; }
            set { RadMenu1.OnClientItemClicked = value; }
        }

        public string OnClientItemOpened
        {
            get { return RadMenu1.OnClientItemOpened; }
            set { RadMenu1.OnClientItemOpened = value; }
        }

        public string OnClientItemOpening
        {
            get { return RadMenu1.OnClientItemOpening; }
            set { RadMenu1.OnClientItemOpening = value; }
        }

        public string OnClientItemClosed
        {
            get { return RadMenu1.OnClientItemClosed; }
            set { RadMenu1.OnClientItemClosed = value; }
        }

        public string OnClientItemClosing
        {
            get { return RadMenu1.OnClientItemClosing; }
            set { RadMenu1.OnClientItemClosing = value; }
        }

        public string OnClientLoad
        {
            get { return RadMenu1.OnClientLoad; }
            set { RadMenu1.OnClientLoad = value; }
        }

        public string PagesToExclude
        {
            get { return _PagesToExclude; }
            set { _PagesToExclude = value; }
        }

        public string RootItemCssClass
        {
            get { return _RootItemCssClass; }
            set { _RootItemCssClass = value; }
        }

        public string RootItemDisabledCssClass
        {
            get { return _RootItemDisabledCssClass; }
            set { _RootItemDisabledCssClass = value; }
        }

        public string RootItemExpandedCssClass
        {
            get { return _RootItemExpandedCssClass; }
            set { _RootItemExpandedCssClass = value; }
        }

        public string RootItemFocusedCssClass
        {
            get { return _RootItemFocusedCssClass; }
            set { _RootItemFocusedCssClass = value; }
        }

        public string RootItemClickedCssClass
        {
            get { return _RootItemClickedCssClass; }
            set { _RootItemClickedCssClass = value; }
        }

        public string RootItemImageUrl
        {
            get { return _RootItemImageUrl; }
            set { _RootItemImageUrl = value; }
        }

        public string RootItemHoveredImageUrl
        {
            get { return _RootItemHoveredImageUrl; }
            set { _RootItemHoveredImageUrl = value; }
        }

        public string RootItemTarget
        {
            get { return _RootItemTarget; }
            set { _RootItemTarget = value; }
        }

        public Color RootItemBackColor
        {
            get { return _RootItemBackColor; }
            set { _RootItemBackColor = value; }
        }

        public Color RootItemBorderColor
        {
            get { return _RootItemBorderColor; }
            set { _RootItemBorderColor = value; }
        }

        public Unit RootItemBorderWidth
        {
            get { return _RootItemBorderWidth; }
            set { _RootItemBorderWidth = value; }
        }

        public BorderStyle RootItemBorderStyle
        {
            get { return _RootItemBorderStyle; }
            set { _RootItemBorderStyle = value; }
        }

        public Color RootItemForeColor
        {
            get { return _RootItemForeColor; }
            set { _RootItemForeColor = value; }
        }

        public Unit RootItemHeight
        {
            get { return _RootItemHeight; }
            set { _RootItemHeight = value; }
        }

        public Unit RootItemWidth
        {
            get { return _RootItemWidth; }
            set { _RootItemWidth = value; }
        }

        public string SelectedPathHeaderItemCss
        {
            get { return _SelectedPathHeaderItemCss; }
            set { _SelectedPathHeaderItemCss = value; }
        }

        public string SelectedPathItemCss
        {
            get { return _SelectedPathItemCss; }
            set { _SelectedPathItemCss = value; }
        }

        public string SelectedPathItemImage
        {
            get { return _SelectedPathItemImage; }
            set { _SelectedPathItemImage = value; }
        }

        public string SelectedPathHeaderItemImage
        {
            get { return _SelectedPathHeaderItemImage; }
            set { _SelectedPathHeaderItemImage = value; }
        }

        public string ShowOnlyCurrent
        {
            get { return _ShowOnlyCurrent; }
            set { _ShowOnlyCurrent = value; }
        }

        public bool ShowPath
        {
            get { return _ShowPath; }
            set { _ShowPath = value; }
        }

        public string Skin
        {
            get { return RadMenu1.Skin; }
            set { RadMenu1.Skin = value; }
        }

        public string Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        public Unit Width
        {
            get { return RadMenu1.Width; }
            set { RadMenu1.Width = value; }
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                //dnnSkinSrc = PortalSettings.ActiveTab.SkinSrc.Replace('\\', '/').Replace("//", "/");
                //dnnSkinPath = dnnSkinSrc.Substring(0, dnnSkinSrc.LastIndexOf('/'));
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            TabController objTabController = new TabController();
            int i = 0;
            int iItemIndex = 0;
            int iRootGroupId = 0;
            qElement temp = default(qElement);
            int StartingItemId = 0;

            AuthPages = new ArrayList();
            PagesQueue = new Queue();
            arrayShowPath = new ArrayList();
            iItemIndex = 0;
            //---------------------------------------------------

            SetMenuProperties();

            if (!Page.IsPostBack)
            {
                //optional code to support displaying a specific branch of the page tree
                GetShowOnlyCurrent(objTabController, ref StartingItemId, ref iRootGroupId);
                int portalID = (PortalSettings.ActiveTab.IsSuperTab ? -1 : PortalSettings.PortalId);
                IList<TabInfo> desktopTabs = TabController.GetTabsBySortOrder(portalID, PortalController.GetActivePortalLanguage(portalID), true);
                for (i = 0; i <= desktopTabs.Count - 1; i++)
                {
                    if (((desktopTabs[i]).TabID == PortalSettings.ActiveTab.TabID))
                    {
                        FillShowPathArray(ref arrayShowPath, (desktopTabs[i]).TabID, objTabController);
                    }

                    //Fariborz Khosravi
                    //if (((desktopTabs[i]).IsVisible && !(desktopTabs[i]).IsDeleted) &&
                    //    (((desktopTabs[i]).StartDate == DateTime.MinValue && (desktopTabs[i]).EndDate == DateTime.MinValue) ||
                    //     ((desktopTabs[i]).StartDate < DateTime.Now && (desktopTabs[i]).EndDate > DateTime.Now) || AdminMode) &&
                    //    (TabPermissionController.CanViewPage(desktopTabs[i]) && !CheckToExclude((desktopTabs[i]).TabName, (desktopTabs[i]).TabID)))
                    if (((desktopTabs[i]).IsVisible && !(desktopTabs[i]).IsDeleted) &&
                        (((desktopTabs[i]).StartDate == DotNetNuke.Common.Utilities.Null.NullDate && (desktopTabs[i]).EndDate == DotNetNuke.Common.Utilities.Null.NullDate) ||
                         ((desktopTabs[i]).StartDate < DateTime.Now && (desktopTabs[i]).EndDate > DateTime.Now) || AdminMode) &&
                        (TabPermissionController.CanViewPage(desktopTabs[i]) && !CheckToExclude((desktopTabs[i]).TabName, (desktopTabs[i]).TabID)))
                    {
                        temp = new qElement();
                        temp.page = (TabInfo)desktopTabs[i];
                        temp.radMenuItem = new RadMenuItem();
                        if (CheckShowOnlyCurrent((desktopTabs[i]).TabID, (desktopTabs[i]).ParentId, StartingItemId, iRootGroupId) && CheckMenuVisibility(desktopTabs[i]))
                        {
                            iItemIndex = iItemIndex + 1;
                            temp.item = iItemIndex;
                            PagesQueue.Enqueue(AuthPages.Count);
                            RadMenu1.Items.Add(temp.radMenuItem);
                        }
                        AuthPages.Add(temp);
                    }
                }

                /*
                if ((ContainHostTabs))
                {
                    IList<TabInfo> hostTabs = TabController.GetTabsBySortOrder(-1, PortalController.GetActivePortalLanguage(-1), true);
                    for (i = 0; i <= hostTabs.Count - 1; i++)
                    {
                        {
                            if ((((TabInfo)hostTabs[i]).TabID == this.PortalSettings.ActiveTab.TabID))
                            {
                                FillShowPathArray(ref arrayShowPath, ((TabInfo)hostTabs[i]).TabID, objTabController);
                            }
                            if ((((TabInfo)hostTabs[i]).IsVisible & !((TabInfo)hostTabs[i]).IsDeleted) & ((((TabInfo)hostTabs[i]).StartDate == Null.NullDate & ((TabInfo)hostTabs[i]).EndDate == Null.NullDate) | (((TabInfo)hostTabs[i]).StartDate < Now & ((TabInfo)hostTabs[i]).EndDate > Now) | AdminMode) & (Permissions.TabPermissionController.CanViewPage(hostTabs[i]) & !CheckToExclude(((TabInfo)hostTabs[i]).TabName, ((TabInfo)hostTabs[i]).TabID)))
                            {
                                temp = new qElement();
                                temp.page = (TabInfo)hostTabs[i];
                                temp.radMenuItem = new RadMenuItem();
                                if (CheckShowOnlyCurrent(((TabInfo)hostTabs[i]).TabID, ((TabInfo)hostTabs[i]).ParentId, StartingItemId, iRootGroupId) && CheckMenuVisibility((TabInfo)hostTabs[i]))
                                {
                                    iItemIndex = iItemIndex + 1;
                                    temp.item = iItemIndex;
                                    PagesQueue.Enqueue(AuthPages.Count);
                                    RadMenu1.Items.Add(temp.radMenuItem);
                                }
                                AuthPages.Add(temp);
                            }
                        }
                    }
                }*/

                //insert first item if enabled
                if ((HeaderFirstItem != ""))
                {
                    RadMenu1.Items.Insert(0, CreateHeaderFirstItem());
                }
                //insert last item if enabled
                if ((HeaderLastItem != ""))
                {
                    RadMenu1.Items.Add(CreateHeaderLastItem());
                }
                BuildMenu(RadMenu1.Items);

                if ((0 == RadMenu1.Items.Count))
                {
                    RadMenu1.Visible = false;
                }
            }
        }

        #endregion

        #region Private Methods

        private bool CheckToExclude(string tabName, int tabId)
        {
            if ((PagesToExclude == ""))
            {
                return false;
            }
            string[] temp = PagesToExclude.Split(new char[] { ',' });
            if ((temp.Length == 0))
            {
                return false;
            }
            foreach (string item in temp)
            {
                try
                {
                    if (tabId == int.Parse(item.Trim()))
                    {
                        return true;
                    }
                }
                catch
                {
                    if (tabName == item.Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void GetShowOnlyCurrent(TabController objTabController, ref int StartingItemId, ref int iRootGroupId)
        {
            StartingItemId = 0;
            iRootGroupId = 0;
            //check if we have a value to work with
            if ((ShowOnlyCurrent == ""))
            {
                return;
            }
            //check if user specified an ID
            if ((char.IsDigit(ShowOnlyCurrent.ToCharArray()[0])))
            {
                int output;
                if (int.TryParse(ShowOnlyCurrent, out output))
                {
                    StartingItemId = output;
                }
            }

            //check if user specified a page name
            if ((ShowOnlyCurrent.StartsWith("PageItem:")))
            {
                TabInfo temptab = objTabController.GetTabByName(ShowOnlyCurrent.Substring(("PageItem:").Length), PortalSettings.PortalId);
                if (((temptab != null)))
                {
                    StartingItemId = temptab.TabID;
                }
            }
            //RootItem
            if (("RootItem" == ShowOnlyCurrent))
            {
                iRootGroupId = PortalSettings.ActiveTab.TabID;
                while (((objTabController.GetTab(iRootGroupId, PortalSettings.PortalId, true)).ParentId != -1))
                {
                    iRootGroupId = (objTabController.GetTab(iRootGroupId, PortalSettings.PortalId, true)).ParentId;
                }
            }
            if ("ChildItems" == ShowOnlyCurrent)
            {
                iRootGroupId = PortalSettings.ActiveTab.TabID;
            }
        }

        private bool CheckShowOnlyCurrent(int tabId, int parentId, int StartingItemId, int iRootGroupId)
        {
            if (("" == ShowOnlyCurrent && parentId == -1) || ("ChildItems" == ShowOnlyCurrent && parentId == PortalSettings.ActiveTab.TabID) || ("CurrentItem" == ShowOnlyCurrent && tabId == PortalSettings.ActiveTab.TabID) || ("RootItem" == ShowOnlyCurrent && iRootGroupId == parentId) || (StartingItemId > 0 && parentId == StartingItemId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckMenuVisibility(TabInfo tab)
        {
            //Fixed: If (Not EnableAdminMenus AndAlso (tab.IsAdminTab Or tab.IsSuperTab)) Then
            if (!EnableAdminMenus && tab.IsSuperTab)
            {
                return false;
            }
            //Fixed: If (Not EnableUserMenus AndAlso Not (tab.IsAdminTab Or tab.IsSuperTab)) Then
            if (!EnableUserMenus && !tab.IsSuperTab)
            {
                return false;
            }
            return true;
        }

        private void FillShowPathArray(ref ArrayList arrayShowPath, int selectedTabID, TabController objTabController)
        {
            TabInfo tempTab = objTabController.GetTab(selectedTabID, PortalSettings.PortalId, true);
            while ((tempTab.ParentId != -1))
            {
                arrayShowPath.Add(tempTab.TabID);
                tempTab = objTabController.GetTab(tempTab.ParentId, PortalSettings.PortalId, true);
            }
            arrayShowPath.Add(tempTab.TabID);
        }

        private void CheckShowPath(int tabId, RadMenuItem menuItemToCheck, string pageName)
        {
            if ((arrayShowPath.Contains(tabId)))
            {
                menuItemToCheck.HighlightPath();
                
                if ((menuItemToCheck.Level > 0))
                {
                    if (!string.IsNullOrEmpty(SelectedPathItemCss))
                    {
                        menuItemToCheck.CssClass = menuItemToCheck.CssClass + " " + SelectedPathItemCss;
                    }
                    if (!string.IsNullOrEmpty(SelectedPathItemImage))
                    {
                        menuItemToCheck.ImageUrl = SelectedPathItemImage.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(SelectedPathHeaderItemCss))
                    {
                        menuItemToCheck.CssClass = menuItemToCheck.CssClass + " " + SelectedPathHeaderItemCss;
                    }
                    if (!string.IsNullOrEmpty(SelectedPathHeaderItemImage))
                    {
                        menuItemToCheck.ImageUrl = SelectedPathHeaderItemImage.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName);
                    }
                }
            }
        }

        private RadMenuItem CreateHeaderSeparatorItem()
        {
            RadMenuItem menuItem = new RadMenuItem();
            menuItem.IsSeparator = true;
            if ((HeaderSeparator != ""))
            {
                //menuItem.ImageUrl = HeaderSeparator.Replace("*SkinPath*", dnnSkinPath);
            }
            if ((HeaderSeparatorCssClass != ""))
            {
                menuItem.CssClass = HeaderSeparatorCssClass;
            }
            if ((HeaderSeparatorCssClassOver != ""))
            {
                menuItem.FocusedCssClass = HeaderSeparatorCssClassOver;
            }
            if ((HeaderSeparatorCssClassClicked != ""))
            {
                menuItem.ClickedCssClass = HeaderSeparatorCssClassClicked;
            }
            return menuItem;
        }

        private RadMenuItem CreateHeaderFirstItem()
        {
            RadMenuItem menuItem = new RadMenuItem();
            menuItem.IsSeparator = true;
            if ((HeaderFirstItem != ""))
            {
                //menuItem.ImageUrl = HeaderFirstItem.Replace("*SkinPath*", dnnSkinPath);
            }
            if ((HeaderFirstItemCssClass != ""))
            {
                menuItem.CssClass = HeaderFirstItemCssClass;
            }
            if ((HeaderFirstItemCssClassOver != ""))
            {
                menuItem.FocusedCssClass = HeaderFirstItemCssClassOver;
            }
            if ((HeaderFirstItemCssClassClicked != ""))
            {
                menuItem.ClickedCssClass = HeaderFirstItemCssClassClicked;
            }
            return menuItem;
        }

        private RadMenuItem CreateHeaderLastItem()
        {
            RadMenuItem menuItem = new RadMenuItem();
            menuItem.IsSeparator = true;
            if ((HeaderLastItem != ""))
            {
                //menuItem.ImageUrl = HeaderLastItem.Replace("*SkinPath*", dnnSkinPath);
            }
            if ((HeaderLastItemCssClass != ""))
            {
                menuItem.CssClass = HeaderLastItemCssClass;
            }
            if ((HeaderLastItemCssClassOver != ""))
            {
                menuItem.FocusedCssClass = HeaderLastItemCssClassOver;
            }
            if ((HeaderLastItemCssClassClicked != ""))
            {
                menuItem.ClickedCssClass = HeaderLastItemCssClassClicked;
            }
            return menuItem;
        }

        private RadMenuItem CreateSeparatorItem()
        {
            RadMenuItem menuItem = new RadMenuItem();
            menuItem.IsSeparator = true;
            if ((ChildGroupSeparator != ""))
            {
                //menuItem.ImageUrl = ChildGroupSeparator.Replace("*SkinPath*", dnnSkinPath);
            }
            if ((ChildGroupSeparatorCssClass != ""))
            {
                menuItem.CssClass = ChildGroupSeparatorCssClass;
            }
            if ((ChildGroupSeparatorCssClassOver != ""))
            {
                menuItem.FocusedCssClass = ChildGroupSeparatorCssClassOver;
            }
            if ((ChildGroupSeparatorCssClassClicked != ""))
            {
                menuItem.ClickedCssClass = ChildGroupSeparatorCssClassClicked;
            }
            return menuItem;
        }

        private RadMenuItem CreateFirstItem()
        {
            RadMenuItem menuItem = new RadMenuItem();
            menuItem.IsSeparator = true;
            if ((ChildGroupFirstItem != ""))
            {
                //menuItem.ImageUrl = ChildGroupFirstItem.Replace("*SkinPath*", dnnSkinPath);
            }
            if ((ChildGroupFirstItemCssClass != ""))
            {
                menuItem.CssClass = ChildGroupFirstItemCssClass;
            }
            if ((ChildGroupFirstItemCssClassOver != ""))
            {
                menuItem.FocusedCssClass = ChildGroupFirstItemCssClassOver;
            }
            if ((ChildGroupFirstItemCssClassClicked != ""))
            {
                menuItem.ClickedCssClass = ChildGroupFirstItemCssClassClicked;
            }
            return menuItem;
        }

        private RadMenuItem CreateLastItem()
        {
            RadMenuItem menuItem = new RadMenuItem();
            menuItem.IsSeparator = true;
            if ((ChildGroupLastItem != ""))
            {
                //menuItem.ImageUrl = ChildGroupLastItem.Replace("*SkinPath*", dnnSkinPath);
            }
            if ((ChildGroupLastItemCssClass != ""))
            {
                menuItem.CssClass = ChildGroupLastItemCssClass;
            }
            if ((ChildGroupLastItemCssClassOver != ""))
            {
                menuItem.FocusedCssClass = ChildGroupLastItemCssClassOver;
            }
            if ((ChildGroupLastItemCssClassClicked != ""))
            {
                menuItem.ClickedCssClass = ChildGroupLastItemCssClassClicked;
            }
            return menuItem;
        }

        private void SetMenuProperties()
        {
            if ((Style != ""))
            {
                Style += "; ";
                try
                {
                    foreach (string cStyle in Style.Split(';'))
                    {
                        if ((cStyle.Trim().Length > 0))
                        {
                            RadMenu1.Style.Add(cStyle.Split(':')[0], cStyle.Split(':')[1]);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void SetGroupProperties(RadMenuItemGroupSettings groupSettings)
        {
            groupSettings.Flow = GroupFlow;
            groupSettings.ExpandDirection = GroupExpandDirection;
            groupSettings.OffsetX = GroupOffsetX;
            groupSettings.OffsetY = GroupOffsetY;

            if ((!GroupWidth.IsEmpty))
            {
                groupSettings.Width = GroupWidth;
            }

            if ((!GroupHeight.IsEmpty))
            {
                groupSettings.Height = GroupHeight;
            }
        }

        private void SetItemProperties(RadMenuItem currentMenuItem, int iLevel, int iItem, string pageName, int pageId)
        {
            string sLevel = EnableLevelCss && iLevel < MaxLevelNumber ? "Level" + iLevel : string.Empty;
            string sItem = iItem <= MaxItemNumber && ((EnableItemCss && iLevel > 0) || (EnableRootItemCss && iLevel == 0)) ? iItem.ToString() : string.Empty;
            object pageIdCssClass = "rmItem" + pageId;
            if ((ItemCssClass != string.Empty))
            {
                currentMenuItem.CssClass = sLevel + ItemCssClass + sItem;
            }

            if ((EnablePageIdCssClass))
            {
                currentMenuItem.CssClass += " " + pageIdCssClass;
            }

            if ((ItemDisabledCssClass != ""))
            {
                currentMenuItem.DisabledCssClass = sLevel + ItemDisabledCssClass + sItem;
            }

            if ((ItemExpandedCssClass != ""))
            {
                currentMenuItem.ExpandedCssClass = sLevel + ItemExpandedCssClass + sItem;
            }

            if ((ItemFocusedCssClass != ""))
            {
                currentMenuItem.FocusedCssClass = sLevel + ItemFocusedCssClass + sItem;
            }

            if ((ItemClickedCssClass != ""))
            {
                currentMenuItem.ClickedCssClass = sLevel + ItemClickedCssClass + sItem;
            }

            if ((ItemImageUrl != ""))
            {
                currentMenuItem.ImageUrl = ItemImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString());
            }

            if ((ItemHoveredImageUrl != ""))
            {
                currentMenuItem.HoveredImageUrl = ItemHoveredImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString());
            }

            if ((ItemTarget != ""))
            {
                currentMenuItem.Target = ItemTarget;
            }

            if ((!ItemBackColor.Equals(Color.Empty)))
            {
                currentMenuItem.BackColor = ItemBackColor;
            }

            if ((!ItemBorderColor.Equals(Color.Empty)))
            {
                currentMenuItem.BorderColor = ItemBorderColor;
            }

            if ((!ItemBorderWidth.IsEmpty))
            {
                currentMenuItem.BorderWidth = ItemBorderWidth;
            }

            if ((ItemBorderStyle != BorderStyle.None))
            {
                currentMenuItem.BorderStyle = ItemBorderStyle;
            }


            if ((!ItemForeColor.Equals(Color.Empty)))
            {
                currentMenuItem.ForeColor = ItemForeColor;
            }

            if ((!ItemHeight.IsEmpty))
            {
                currentMenuItem.Height = ItemHeight;
            }

            if ((!ItemWidth.IsEmpty))
            {
                currentMenuItem.Width = ItemWidth;
            }
        }

        private void SetRootItemProperties(RadMenuItem currentMenuItem, int iLevel, int iItem, string pageName, int pageId)
        {
            string sLevel = EnableLevelCss && iLevel < MaxLevelNumber ? "Level" + iLevel : string.Empty;
            string sItem = iItem <= MaxItemNumber && ((EnableItemCss && iLevel > 0) || (EnableRootItemCss && iLevel == 0)) ? iItem.ToString() : string.Empty;
            object pageIdCssClass = "rmPageId" + pageId;

            if ((RootItemCssClass != ""))
            {
                currentMenuItem.CssClass = sLevel + RootItemCssClass + sItem;
            }

            if ((EnablePageIdCssClass))
            {
                currentMenuItem.CssClass += " " + pageIdCssClass;
            }

            if ((RootItemDisabledCssClass != ""))
            {
                currentMenuItem.DisabledCssClass = sLevel + RootItemDisabledCssClass + sItem;
            }

            if ((RootItemExpandedCssClass != ""))
            {
                currentMenuItem.ExpandedCssClass = sLevel + RootItemExpandedCssClass + sItem;
            }

            if ((RootItemFocusedCssClass != ""))
            {
                currentMenuItem.FocusedCssClass = sLevel + RootItemFocusedCssClass + sItem;
            }

            if ((RootItemClickedCssClass != ""))
            {
                currentMenuItem.ClickedCssClass = sLevel + RootItemClickedCssClass + sItem;
            }

            if ((RootItemImageUrl != ""))
            {
                currentMenuItem.ImageUrl = RootItemImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString());
            }

            if ((RootItemHoveredImageUrl != ""))
            {
                currentMenuItem.HoveredImageUrl = RootItemHoveredImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString());
            }

            if ((RootItemTarget != ""))
            {
                currentMenuItem.Target = RootItemTarget;
            }

            if ((!RootItemBackColor.Equals(Color.Empty)))
            {
                currentMenuItem.BackColor = RootItemBackColor;
            }

            if ((!RootItemBorderColor.Equals(Color.Empty)))
            {
                currentMenuItem.BorderColor = RootItemBorderColor;
            }

            if ((!RootItemBorderWidth.IsEmpty))
            {
                currentMenuItem.BorderWidth = RootItemBorderWidth;
            }

            if ((RootItemBorderStyle != BorderStyle.None))
            {
                currentMenuItem.BorderStyle = RootItemBorderStyle;
            }

            if ((!RootItemForeColor.Equals(Color.Empty)))
            {
                currentMenuItem.ForeColor = RootItemForeColor;
            }

            if ((!RootItemHeight.IsEmpty))
            {
                currentMenuItem.Height = RootItemHeight;
            }


            if ((!RootItemWidth.IsEmpty))
            {
                currentMenuItem.Width = RootItemWidth;
            }

        }

        private void BuildMenu(RadMenuItemCollection rootCollection)
        {
            qElement temp = default(qElement);
            qElement temp2 = default(qElement);
            TabInfo page = default(TabInfo);
            int pageID = 0;
            int j = 0;
            int iItemIndex = 0;

            while (!(PagesQueue.Count == 0))
            {
                pageID = Convert.ToInt32(PagesQueue.Dequeue());
                temp = (qElement)AuthPages[pageID];
                page = (TabInfo)temp.page;

                if ((UseTitle && !string.IsNullOrEmpty(page.Title)))
                {
                    temp.radMenuItem.Text = page.Title;
                }
                else
                {
                    temp.radMenuItem.Text = page.LocalizedTabName;
                }

                if ((!string.IsNullOrEmpty(page.IconFile) & EnablePageIcons))
                {
                    temp.radMenuItem.ImageUrl = DotNetNuke.Common.Globals.ResolveUrl(page.IconFile);
                }

                if ((!page.DisableLink))
                {
                    if ((page.FullUrl.StartsWith("*") & page.FullUrl.IndexOf("*", 1) != -1))
                    {
                        temp.radMenuItem.NavigateUrl = page.FullUrl.Substring(page.FullUrl.IndexOf("*", 1) + 1);
                        temp.radMenuItem.Target = page.FullUrl.Substring(1, page.FullUrl.IndexOf("*", 1) - 1);
                    }
                    else
                    {
                        temp.radMenuItem.NavigateUrl = page.FullUrl;
                    }
                }
                else if ((CopyChildItemLink && page.Level >= MaxLevel))
                {
                    j = 0;
                    //check if there are any child items and use a href from the first one
                    while ((j < AuthPages.Count && (((qElement)AuthPages[j]).page.ParentId != page.TabID || ((qElement)AuthPages[j]).page.DisableLink)))
                    {
                        j = j + 1;
                    }
                    if ((j < AuthPages.Count))
                    {
                        // child item found. use its link
                        temp.radMenuItem.NavigateUrl = ((qElement)AuthPages[j]).page.FullUrl;
                    }
                }
                if ((EnableToolTips & !string.IsNullOrEmpty(page.Description)))
                {
                    temp.radMenuItem.ToolTip = page.Description;
                }

                //set all other item properties
                if ((temp.radMenuItem.Level == 0))
                {
                    SetRootItemProperties(temp.radMenuItem, page.Level, temp.item, page.TabName, page.TabID);
                }
                else
                {
                    SetItemProperties(temp.radMenuItem, page.Level, temp.item, page.TabName, page.TabID);
                }

                //check showpath
                if ((ShowPath))
                {
                    CheckShowPath(page.TabID, temp.radMenuItem, page.TabName);
                }


                //image-only menu check
                if ((ImagesOnlyMenu & temp.radMenuItem.ImageUrl != ""))
                {
                    temp.radMenuItem.Text = "";
                }

                //attach child items the current one
                if ((page.Level < MaxLevel | MaxLevel < 0))
                {
                    iItemIndex = 0;
                    //set all group properties
                    SetGroupProperties(temp.radMenuItem.GroupSettings);
                    for (j = 0; j <= AuthPages.Count - 1; j++)
                    {
                        temp2 = (qElement)AuthPages[j];
                        if ((temp2.page.ParentId == page.TabID))
                        {
                            // add a separator item if enabled
                            if ((temp.radMenuItem.Items.Count > 0 & ChildGroupSeparator != ""))
                            {
                                temp.radMenuItem.Items.Add(CreateSeparatorItem());
                            }
                            temp.radMenuItem.Items.Add(temp2.radMenuItem);
                            PagesQueue.Enqueue(j);
                            iItemIndex = iItemIndex + 1;
                            temp2.item = iItemIndex;
                            AuthPages[j] = temp2;
                        }
                    }
                    if ((temp.radMenuItem.Items.Count > 0))
                    {
                        //insert first item if enabled
                        if ((ChildGroupFirstItem != ""))
                        {
                            temp.radMenuItem.Items.Insert(0, CreateFirstItem());
                        }
                        //insert last item if enabled
                        if ((ChildGroupLastItem != ""))
                        {
                            temp.radMenuItem.Items.Add(CreateLastItem());
                        }
                    }
                }
            }
        }

        #endregion

    }
}