Type.registerNamespace("Telerik.Web.UI");
$telerik.findDatePicker = $find;
$telerik.toDatePicker = function (a) {
    return a;
};
Telerik.Web.UI.RadDatePicker = function (a) {
    Telerik.Web.UI.RadDatePicker.initializeBase(this, [a]);
    this._calendar = null;
    this._dateInput = null;
    this._popupButton = null;
    this._validationInput = null;
    this._popupDirection = Telerik.Web.RadDatePickerPopupDirection.BottomRight;
    this._enableScreenBoundaryDetection = true;
    this._zIndex = null;
    this._enableShadows = true;
    this._animationSettings = {};
    this._popupControlID = null;
    this._popupButtonSettings = null;
    this._focusedDate = new Date(1980, 0, 1);
    this._minDate = new Date(1980, 0, 1);
    this._maxDate = new Date(2099, 11, 31);
    this._enabled = true;
    this._originalDisplay = null;
    this._showPopupOnFocus = false;
    this._enableAriaSupport = false;
    this._onPopupImageMouseOverDelegate = null;
    this._onPopupImageMouseOutDelegate = null;
    this._onPopupButtonClickDelegate = null;
    this._onPopupButtonKeyPressDelegate = null;
    this._onDateInputFocusDelegate = null;
};
Telerik.Web.UI.RadDatePicker.PopupInstances = {};
Telerik.Web.UI.RadDatePicker.prototype = {
    initialize: function () {
        Telerik.Web.UI.RadDatePicker.callBaseMethod(this, "initialize");
        this._initializeDateInput();
        this._initializeCalendar();
        var b = $get(this.get_id() + "_wrapper");
        if (($telerik.isIE7 || $telerik.quirksMode) && b.style.display == "inline-block") {
            b.style.display = "inline";
            b.style.zoom = 1;
        } if ($telerik.isIE && document.documentMode && document.documentMode > 7 && b.style.display == "inline") {
            b.style.display = "inline-block";
            this.get_dateInput().repaint();
        } if ($telerik.getCurrentStyle(b, "direction") == "rtl") {
            var a = this.get_dateInput()._skin != "" ? String.format(" RadPickerRTL_{0}", this.get_dateInput()._skin) : "";
            b.className += String.format(" RadPickerRTL{0}", a);
        } this._refreshPopupShadowSetting();
        this.CalendarSelectionInProgress = false;
        this.InputSelectionInProgress = false;
        if (this.get_enableAriaSupport()) {
            this._initializeAriaSupport();
        }
    }, dispose: function () {
        if (this._calendar != null) {
            this.hidePopup();
            this._calendar.dispose();
        } if (this._popupButton != null) {
            var b = this.get__popupImage();
            if (b != null) {
                if (this._onPopupImageMouseOverDelegate) {
                    try {
                        $removeHandler(b, "mouseover", this._onPopupImageMouseOverDelegate);
                    } catch (a) { } this._onPopupImageMouseOverDelegate = null;
                } if (this._onPopupImageMouseOutDelegate) {
                    try {
                        $removeHandler(b, "mouseout", this._onPopupImageMouseOutDelegate);
                    } catch (a) { } this._onPopupImageMouseOutDelegate = null;
                }
            } if (this._onPopupButtonClickDelegate) {
                try {
                    $removeHandler(this._popupButton, "click", this._onPopupButtonClickDelegate);
                } catch (a) { } this._onPopupButtonClickDelegate = null;
            } if (this._onPopupButtonKeyPressDelegate) {
                try {
                    $removeHandler(this._popupButton, "keypress", this._onPopupButtonKeyPressDelegate);
                } catch (a) { } this._onPopupButtonKeyPressDelegate = null;
            }
        } if (this._popupButton) {
            this._popupButton._events = null;
        } Telerik.Web.UI.RadDatePicker.callBaseMethod(this, "dispose");
    }, clear: function () {
        if (this._dateInput) {
            this._dateInput.clear();
        } if (this._calendar) {
            this._calendar.unselectDates(this._calendar.get_selectedDates());
        }
    }, _clearHovers: function () {
        var b = this.get_popupContainer().getElementsByTagName("td");
        for (var a = 0;
        a < b.length;
        a++) {
            if (b[a].className && b[a].className.indexOf("rcHover") != -1) {
                b[a].className = b[a].className.replace("rcHover", "");
            }
        }
    }, togglePopup: function () {
        if (this.isPopupVisible()) {
            this.hidePopup();
        } else {
            this.showPopup();
        } return false;
    }, isPopupVisible: function () {
        if (!this._calendar) {
            return false;
        } return this.get__popup().IsVisible() && (this.get__popup().Opener == this);
    }, showPopup: function (h, j) {
        if (this.isPopupVisible() || !this._calendar) {
            return;
        } this._actionBeforeShowPopup();
        this.get__popup().ExcludeFromHiding = this.get__PopupVisibleControls();
        this.hidePopup();
        var g = true;
        var a = new Telerik.Web.UI.DatePickerPopupOpeningEventArgs(this._calendar, false);
        this.raise_popupOpening(a);
        if (a.get_cancel() == true) {
            return;
        } g = !a.get_cancelCalendarSynchronization();
        this._clearHovers();
        this.get__popup().Opener = this;
        this.get__popup().Show(h, j, this.get_popupContainer());
        if (g == true) {
            var c = this._dateInput.get_selectedDate();
            if (this.isEmpty() || (!c)) {
                this._focusCalendar();
            } else {
                this._setCalendarDate(c);
            }
        } if (this._calendar && !this._calendar._linksHandlersAdded) {
            var f = this._calendar.get_element().getElementsByTagName("a");
            for (var b = 0, d = f.length;
            b < d;
            b++) {
                var e = f[b];
                $addHandlers(e, { click: Function.createDelegate(this, this._click) });
            } this._calendar._linksHandlersAdded = true;
        } if ((this._calendar._enableKeyboardNavigation) && (!this._calendar._enableMultiSelect)) {
            this._calendar.CurrentViews[0].DomTable.tabIndex = 100;
            this._calendar.CurrentViews[0].DomTable.focus();
        }
    }, _click: function (b) {
        var a = (b.srcElement) ? b.srcElement : b.target;
        if (a.tagName && a.tagName.toLowerCase() == "a") {
            var c = a.getAttribute("href", 2);
            if (c == "#" || (location.href + "#" == c)) {
                if (b.preventDefault) {
                    b.preventDefault();
                } return false;
            }
        }
    }, isEmpty: function () {
        return this._dateInput.isEmpty();
    }, hidePopup: function () {
        if (!this.get_calendar()) {
            return false;
        } this._hideFastNavigationPopup(this);
        if (this.get__popup().IsVisible()) {
            var a = this.get__popup().Hide();
            if (a == false) {
                return false;
            }
        } return true;
    }, getElementDimensions: function (a) {
        return Telerik.Web.UI.Calendar.Utils.GetElementDimensions(a);
    }, getElementPosition: function (a) {
        return $telerik.getLocation(a);
    }, get_calendar: function () {
        return this._calendar;
    }, set_calendar: function (a) {
        this._calendar = a;
    }, get_popupButton: function () {
        return this._popupButton;
    }, get_dateInput: function () {
        return this._dateInput;
    }, set_dateInput: function (a) {
        this._dateInput = a;
    }, get_textBox: function () {
        return this._dateInput._textBoxElement;
    }, get_popupContainer: function () {
        if ((this._popupContainer == null)) {
            if (this._popupContainerID) {
                this._popupContainer = $get(this._popupContainerID);
            } else {
                this._popupContainer = null;
            }
        } return this._popupContainer;
    }, get_enabled: function () {
        return this._enabled;
    }, set_enabled: function (c) {
        if (this._enabled != c) {
            var a = this.get_popupButton();
            var b = this.get__popupImage();
            if (c) {
                this._enabled = true;
                if (this._dateInput) {
                    this._dateInput.enable();
                } if (this._calendar) {
                    this._calendar.set_enabled(true);
                } if (a) {
                    Sys.UI.DomElement.removeCssClass(a, "rcDisabled");
                    a.setAttribute("href", "#");
                } if (this._onPopupButtonClickDelegate) {
                    $addHandler(a, "click", this._onPopupButtonClickDelegate);
                } else {
                    if (a) {
                        this._onPopupButtonClickDelegate = Function.createDelegate(this, this._onPopupButtonClickHandler);
                        $addHandler(a, "click", this._onPopupButtonClickDelegate);
                    }
                } if (this._onPopupButtonKeyPressDelegate) {
                    $addHandler(a, "keypress", this._onPopupButtonKeyPressDelegate);
                } if (this._onPopupImageMouseOverDelegate) {
                    $addHandler(b, "mouseover", this._onPopupImageMouseOverDelegate);
                } if (this._onPopupImageMouseOutDelegate) {
                    $addHandler(b, "mouseout", this._onPopupImageMouseOutDelegate);
                } var d = $get(this.get_id() + "_wrapper");
                if (d.attributes.disabled) {
                    d.removeAttribute("disabled");
                }
            } else {
                this.hidePopup();
                this._enabled = false;
                if (this._dateInput) {
                    this._dateInput.disable();
                } if (this._onPopupButtonClickDelegate) {
                    $removeHandler(a, "click", this._onPopupButtonClickDelegate);
                } if (this._onPopupButtonKeyPressDelegate) {
                    $removeHandler(a, "keypress", this._onPopupButtonKeyPressDelegate);
                } if (this._onPopupImageMouseOverDelegate) {
                    $removeHandler(b, "mouseover", this._onPopupImageMouseOverDelegate);
                } if (this._onPopupImageMouseOutDelegate) {
                    $removeHandler(b, "mouseout", this._onPopupImageMouseOutDelegate);
                } if (a) {
                    Sys.UI.DomElement.addCssClass(a, "rcDisabled");
                    a.removeAttribute("href");
                }
            } this.raisePropertyChanged("enabled");
        }
    }, get_visible: function () {
        var a = $get(this.get_id() + "_wrapper");
        if (a.style.display == "none") {
            return false;
        } else {
            return true;
        }
    }, set_visible: function (a) {
        var b = $get(this.get_id() + "_wrapper");
        if (a == true && this._originalDisplay != null) {
            b.style.display = this._originalDisplay;
            this.repaint();
        } else {
            if (a == false && this.get_visible()) {
                this._originalDisplay = b.style.display;
                b.style.display = "none";
            }
        }
    }, get_selectedDate: function () {
        return this._dateInput.get_selectedDate();
    }, set_selectedDate: function (a) {
        this._dateInput.set_selectedDate(a);
    }, get_minDate: function () {
        return this._minDate;
    }, set_minDate: function (d) {
        var c = this._cloneDate(d);
        if (this._minDate.toString() != c.toString()) {
            if (!this._dateInput) {
                this._minDate = c;
            } else {
                var a = false;
                if (this.isEmpty()) {
                    a = true;
                } this._minDate = c;
                this._dateInput.set_minDate(c);
                if (this.get_focusedDate() < c) {
                    this.set_focusedDate(c);
                } var b = [c.getFullYear(), (c.getMonth() + 1), c.getDate()];
                if (this._calendar) {
                    this._calendar.set_rangeMinDate(b);
                }
            } this.updateClientState();
            this.raisePropertyChanged("minDate");
        }
    }, get_minDateStr: function () {
        var a = this._minDate.getFullYear().toString();
        while (a.length < 4) {
            a = "0" + a;
        } return parseInt(this._minDate.getMonth() + 1) + "/" + this._minDate.getDate() + "/" + a + " " + this._minDate.getHours() + ":" + this._minDate.getMinutes() + ":" + this._minDate.getSeconds();
    }, get_maxDate: function () {
        return this._maxDate;
    }, set_maxDate: function (c) {
        var b = this._cloneDate(c);
        if (this._maxDate.toString() != b.toString()) {
            if (!this._dateInput) {
                this._maxDate = b;
            } else {
                this._maxDate = b;
                this._dateInput.set_maxDate(b);
                if (this.get_focusedDate() > b) {
                    this.set_focusedDate(b);
                } var a = [b.getFullYear(), (b.getMonth() + 1), b.getDate()];
                if (this._calendar) {
                    this._calendar.set_rangeMaxDate(a);
                }
            } this.updateClientState();
            this.raisePropertyChanged("maxDate");
        }
    }, get_maxDateStr: function () {
        var a = this._maxDate.getFullYear().toString();
        while (a.length < 4) {
            a = "0" + a;
        } return parseInt(this._maxDate.getMonth() + 1) + "/" + this._maxDate.getDate() + "/" + a + " " + this._maxDate.getHours() + ":" + this._maxDate.getMinutes() + ":" + this._maxDate.getSeconds();
    }, get_focusedDate: function () {
        return this._focusedDate;
    }, set_focusedDate: function (b) {
        var a = this._cloneDate(b);
        if (this._focusedDate.toString() != a.toString()) {
            this._focusedDate = a;
            this.raisePropertyChanged("focusedDate");
        }
    }, get_showPopupOnFocus: function () {
        return this._showPopupOnFocus;
    }, set_showPopupOnFocus: function (a) {
        this._showPopupOnFocus = a;
    }, get_enableAriaSupport: function () {
        return this._enableAriaSupport;
    }, set_enableAriaSupport: function (a) {
        if (this._enableAriaSupport != a) {
            this._enableAriaSupport = a;
        }
    }, repaint: function () {
        this._updatePercentageHeight();
    }, get_popupDirection: function () {
        return this._popupDirection;
    }, set_popupDirection: function (a) {
        this._popupDirection = a;
    }, get_enableScreenBoundaryDetection: function () {
        return this._enableScreenBoundaryDetection;
    }, set_enableScreenBoundaryDetection: function (a) {
        this._enableScreenBoundaryDetection = a;
    }, saveClientState: function (a) {
        var e = ["minDateStr", "maxDateStr"];
        if (a) {
            for (var c = 0, d = a.length;
            c < d;
            c++) {
                e[e.length] = a[c];
            }
        } var f = {};
        var b;
        for (var c = 0;
        c < e.length;
        c++) {
            b = e[c];
            switch (b) {
                case "minDateStr": f[b] = this.get_minDate().format("yyyy-MM-dd-HH-mm-ss");
                    break;
                case "maxDateStr": f[b] = this.get_maxDate().format("yyyy-MM-dd-HH-mm-ss");
                    break;
                default: f[b] = this["get_" + b]();
                    break;
            }
        } return Sys.Serialization.JavaScriptSerializer.serialize(f);
    }, _initializeDateInput: function () {
        if (this._dateInput != null && (!this._dateInput.get_owner)) {
            var a = this;
            this._dateInput.get_owner = function () {
                return a;
            };
            this._dateInput.Owner = this;
            this._setUpValidationInput();
            this._setUpDateInput();
            this._propagateRangeValues();
            this._initializePopupButton();
        } this._updatePercentageHeight();
    }, _updatePercentageHeight: function () {
        var b = $get(this.get_id() + "_wrapper");
        if (b.style.height.indexOf("%") != -1 && b.offsetHeight > 0) {
            var a = 0;
            if (this.get_dateInput()._textBoxElement.currentStyle) {
                a = parseInt(this.get_dateInput()._textBoxElement.currentStyle.borderTopWidth) + parseInt(this.get_dateInput()._textBoxElement.currentStyle.borderBottomWidth) + parseInt(this.get_dateInput()._textBoxElement.currentStyle.paddingTop) + parseInt(this.get_dateInput()._textBoxElement.currentStyle.paddingBottom);
            } else {
                if (window.getComputedStyle) {
                    a = parseInt(window.getComputedStyle(this.get_dateInput()._textBoxElement, null).getPropertyValue("border-top-width")) + parseInt(window.getComputedStyle(this.get_dateInput()._textBoxElement, null).getPropertyValue("border-bottom-width")) + parseInt(window.getComputedStyle(this.get_dateInput()._textBoxElement, null).getPropertyValue("padding-top")) + parseInt(window.getComputedStyle(this.get_dateInput()._textBoxElement, null).getPropertyValue("padding-bottom"));
                }
            } this.get_dateInput()._textBoxElement.style.height = "1px";
            this.get_dateInput()._textBoxElement.style.cssText = this.get_dateInput()._textBoxElement.style.cssText;
            this.get_dateInput()._textBoxElement.style.height = b.offsetHeight - a + "px";
            if (this.get_dateInput()._originalTextBoxCssText.search(/(^|[^-])height/) != -1) {
                this.get_dateInput()._originalTextBoxCssText = this.get_dateInput()._originalTextBoxCssText.replace(/(^|[^-])height(\s*):(\s*)([^;]+);/i, "$1height:" + (b.offsetHeight - a) + "px;");
            } else {
                this.get_dateInput()._originalTextBoxCssText += "height:" + (b.offsetHeight - a) + "px;";
            }
        }
    }, _initializeCalendar: function () {
        if (this._calendar != null) {
            this._setUpCalendar();
            this._calendar.set_enableMultiSelect(false);
            this._calendar.set_useColumnHeadersAsSelectors(false);
            this._calendar.set_useRowHeadersAsSelectors(false);
            if (this._zIndex) {
                this._calendar._zIndex = parseInt(this._zIndex, 10) + 2;
            } this._calendar._enableShadows = this._enableShadows;
            this._popupContainerID = this._calendar.get_id() + "_wrapper";
        }
    }, _propagateRangeValues: function () {
        if (this.get_minDate().toString() != new Date(1980, 0, 1)) {
            this._dateInput._minDate = this.get_minDate();
        } if (this.get_maxDate().toString() != new Date(2099, 11, 31)) {
            this._dateInput._maxDate = this.get_maxDate();
        }
    }, _triggerDomChangeEvent: function () {
        this._dateInput._triggerDomEvent("change", this._validationInput);
    }, _initializeAriaSupport: function () {
        var b = document.getElementById(this.get_id() + "_wrapper");
        b.setAttribute("aria-atomic", "true");
        var a = document.getElementById(this.get_id() + "_popupButton");
        if (a) {
            a.setAttribute("role", "button");
            a.setAttribute("aria-controls", this.get_calendar().get_id() + "_wrapper");
        }
    }, _initializePopupButton: function () {
        this._popupButton = $get(this._popupControlID);
        if (this._popupButton != null) {
            this._attachPopupButtonEvents();
        }
    }, _attachPopupButtonEvents: function () {
        var b = this.get__popupImage();
        var a = this;
        if (b != null) {
            if (!this._hasAttribute("onmouseover")) {
                this._onPopupImageMouseOverDelegate = Function.createDelegate(this, this._onPopupImageMouseOverHandler);
                $addHandler(b, "mouseover", this._onPopupImageMouseOverDelegate);
            } if (!this._hasAttribute("onmouseout")) {
                this._onPopupImageMouseOutDelegate = Function.createDelegate(this, this._onPopupImageMouseOutHandler);
                $addHandler(b, "mouseout", this._onPopupImageMouseOutDelegate);
            }
        } if (this._hasAttribute("href") != null && this._hasAttribute("href") != "" && this._hasAttribute("onclick") == null) {
            this._onPopupButtonClickDelegate = Function.createDelegate(this, this._onPopupButtonClickHandler);
            $addHandler(this._popupButton, "click", this._onPopupButtonClickDelegate);
        } if (this._popupButton) {
            this._onPopupButtonKeyPressDelegate = Function.createDelegate(this, this._onPopupButtonKeyPressHandler);
            $addHandler(this._popupButton, "keypress", this._onPopupButtonKeyPressDelegate);
        }
    }, _onPopupImageMouseOverHandler: function (a) {
        this.get__popupImage().src = this._popupButtonSettings.ResolvedHoverImageUrl;
    }, _onPopupImageMouseOutHandler: function (a) {
        this.get__popupImage().src = this._popupButtonSettings.ResolvedImageUrl;
    }, _onPopupButtonClickHandler: function (a) {
        this.togglePopup();
        a.stopPropagation();
        a.preventDefault();
        return false;
    }, _onPopupButtonKeyPressHandler: function (a) {
        if (a.charCode == 32) {
            this.togglePopup();
            a.stopPropagation();
            a.preventDefault();
            return false;
        }
    }, _hasAttribute: function (a) {
        return this._popupButton.getAttribute(a);
    }, _calendarDateSelected: function (b) {
        if (this.InputSelectionInProgress == true) {
            return;
        } if (b.IsSelected) {
            if (this.hidePopup() == false) {
                return;
            } var a = this._getJavaScriptDate(b.get_date());
            this.CalendarSelectionInProgress = true;
            this._setInputDate(a);
        }
    }, _actionBeforeShowPopup: function () {
        for (var b in Telerik.Web.UI.RadDatePicker.PopupInstances) {
            if (Telerik.Web.UI.RadDatePicker.PopupInstances.hasOwnProperty(b)) {
                var a = Telerik.Web.UI.RadDatePicker.PopupInstances[b].Opener;
                this._hideFastNavigationPopup(a);
                Telerik.Web.UI.RadDatePicker.PopupInstances[b].Hide();
            }
        }
    }, _hideFastNavigationPopup: function (b) {
        if (b) {
            var a = b.get_calendar()._getFastNavigation().Popup;
            if (a && a.IsVisible()) {
                a.Hide(true);
            }
        }
    }, _setInputDate: function (a) {
        this._dateInput.set_selectedDate(a);
    }, _getJavaScriptDate: function (b) {
        /*Fariborz Khosravi*/
        b = DnnExpert.Util.PersianToGregorian(b[0], b[1], b[2]);
        var a = new Date();
        a.setFullYear(b[0], b[1] - 1, b[2]);
        return a;
    }, _onDateInputDateChanged: function (b, a) {
        this._setValidatorDate(a.get_newDate());
        this._triggerDomChangeEvent();
        if (!this.isPopupVisible()) {
            return;
        } if (this.isEmpty()) {
            this._focusCalendar();
        } else {
            if (!this.CalendarSelectionInProgress) {
                this._setCalendarDate(a.get_newDate());
            }
        }
    }, _focusCalendar: function () {
        this._calendar.unselectDates(this._calendar.get_selectedDates());
        var a = [this.get_focusedDate().getFullYear(), this.get_focusedDate().getMonth() + 1, this.get_focusedDate().getDate()];
        this._calendar.navigateToDate(a);
    }, _setValidatorDate: function (c) {
        var d = "";
        if (c != null) {
            var b = (c.getMonth() + 1).toString();
            if (b.length == 1) {
                b = "0" + b;
            } var a = c.getDate().toString();
            if (a.length == 1) {
                a = "0" + a;
            } d = c.getFullYear() + "-" + b + "-" + a;
        } this._validationInput.value = d;
    }, _setCalendarDate: function (b) {
        var a = [b.getFullYear(), b.getMonth() + 1, b.getDate()];
        var c = (this._calendar.FocusedDate[1] != a[1]) || (this._calendar.FocusedDate[0] != a[0]);
        this.InputSelectionInProgress = true;
        this._calendar.unselectDates(this._calendar.get_selectedDates());
        this._calendar.selectDate(a, c);
        this.InputSelectionInProgress = false;
    }, _cloneDate: function (b) {
        var c = null;
        if (!b) {
            return null;
        } if (typeof (b.setFullYear) == "function") {
            c = [];
            c[c.length] = b.getFullYear();
            c[c.length] = b.getMonth() + 1;
            c[c.length] = b.getDate();
            c[c.length] = b.getHours();
            c[c.length] = b.getMinutes();
            c[c.length] = b.getSeconds();
            c[c.length] = b.getMilliseconds();
        } else {
            if (typeof (b) == "string") {
                c = b.split(/-/);
            }
        } if (c != null) {
            var a = new Date();
            a.setDate(1);
            a.setFullYear(c[0]);
            a.setMonth(c[1] - 1);
            a.setDate(c[2]);
            a.setHours(c[3]);
            a.setMinutes(c[4]);
            a.setSeconds(c[5]);
            a.setMilliseconds(0);
            return a;
        } return null;
    }, _setUpValidationInput: function () {
        this._validationInput = $get(this.get_id());
    }, _setUpDateInput: function () {
        this._onDateInputValueChangedDelegate = Function.createDelegate(this, this._onDateInputValueChangedHandler);
        this._dateInput.add_valueChanged(this._onDateInputValueChangedDelegate);
        this._onDateInputBlurDelegate = Function.createDelegate(this, this._onDateInputBlurHandler);
        this._dateInput.add_blur(this._onDateInputBlurDelegate);
        this._onDateInputKeyPressDelegate = Function.createDelegate(this, this._onDateInputKeyPressHandler);
        this._dateInput.add_keyPress(this._onDateInputKeyPressDelegate);
        this._onDateInputFocusDelegate = Function.createDelegate(this, this._onDateInputFocusHandler);
        this._dateInput.add_focus(this._onDateInputFocusDelegate);
    }, _onDateInputValueChangedHandler: function (b, a) {
        this._onDateInputDateChanged(b, a);
        this.raise_dateSelected(a);
        this.CalendarSelectionInProgress = false;
    }, _onDateInputBlurHandler: function (b, a) {
        if (!b.get_selectedDate()) {
            this._validationInput.value = "";
        }
    }, _onDateInputFocusHandler: function (b, a) {
        if (this._calendar && this.get_showPopupOnFocus()) {
            this.showPopup();
        }
    }, _triggerDomEvent: function (b, d) {
        if (!b || b == "" || !d) {
            return;
        } if (d.fireEvent && document.createEventObject) {
            var c = document.createEventObject();
            d.fireEvent(String.format("on{0}", b), c);
        } else {
            if (d.dispatchEvent) {
                var a = true;
                var c = document.createEvent("HTMLEvents");
                c.initEvent(b, a, true);
                d.dispatchEvent(c);
            }
        }
    }, _onDateInputKeyPressHandler: function (b, a) {
        if (a.get_keyCode() == 13) {
            this._setValidatorDate(b.get_selectedDate());
        }
    }, _setUpCalendar: function () {
        this._onCalendarDateSelectedDelegate = Function.createDelegate(this, this._onCalendarDateSelectedHandler);
        this._calendar.add_dateSelected(this._onCalendarDateSelectedDelegate);
    }, _onCalendarDateSelectedHandler: function (b, a) {
        if (this.isPopupVisible()) {
            this._calendarDateSelected(a.get_renderDay());
        }
    }, get__popupImage: function () {
        var b = null;
        if (this._popupButton != null) {
            var a = this._popupButton.getElementsByTagName("img");
            if (a.length > 0) {
                b = a[0];
            } else {
                b = this._popupButton;
            }
        } return b;
    }, _refreshPopupShadowSetting: function () {
        if (!this.get_calendar()) {
            return;
        } var a = Telerik.Web.UI.RadDatePicker.PopupInstances[this.get_calendar().get_id()];
        if (a && !$telerik.quirksMode) {
            this.get__popup().EnableShadows = this._enableShadows;
        }
    }, get__popup: function () {
        var a = Telerik.Web.UI.RadDatePicker.PopupInstances[this.get_calendar().get_id()];
        if (!a) {
            a = new Telerik.Web.UI.Calendar.Popup();
            if (this._zIndex) {
                a.zIndex = this._zIndex;
            } if (!this._enableShadows) {
                a.EnableShadows = false;
            } if (this._animationSettings) {
                a.ShowAnimationDuration = this._animationSettings.ShowAnimationDuration;
                a.ShowAnimationType = this._animationSettings.ShowAnimationType;
                a.HideAnimationDuration = this._animationSettings.HideAnimationDuration;
                a.HideAnimationType = this._animationSettings.HideAnimationType;
            } Telerik.Web.UI.RadDatePicker.PopupInstances[this._calendar.get_id()] = a;
        } return a;
    }, get__PopupVisibleControls: function () {
        var a = [this.get_textBox(), this.get_popupContainer()];
        if (this._popupButton != null) {
            a[a.length] = this._popupButton;
        } return a;
    }, get__PopupButtonSettings: function () {
        return this._popupButtonSettings;
    }, set__PopupButtonSettings: function (a) {
        this._popupButtonSettings = a;
    }, add_dateSelected: function (a) {
        this.get_events().addHandler("dateSelected", a);
    }, remove_dateSelected: function (a) {
        this.get_events().removeHandler("dateSelected", a);
    }, raise_dateSelected: function (a) {
        this.raiseEvent("dateSelected", a);
    }, add_popupOpening: function (a) {
        this.get_events().addHandler("popupOpening", a);
    }, remove_popupOpening: function (a) {
        this.get_events().removeHandler("popupOpening", a);
    }, raise_popupOpening: function (a) {
        this.raiseEvent("popupOpening", a);
    }, add_popupClosing: function (a) {
        this.get_events().addHandler("popupClosing", a);
    }, remove_popupClosing: function (a) {
        this.get_events().removeHandler("popupClosing", a);
    }, raise_popupClosing: function (a) {
        this.raiseEvent("popupClosing", a);
    }
};
Telerik.Web.UI.RadDatePicker.registerClass("Telerik.Web.UI.RadDatePicker", Telerik.Web.UI.RadWebControl);
