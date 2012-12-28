﻿/*http://keith-wood.name/calendars.html   Islamic calendar for jQuery v1.1.4.   Written by Keith Wood (kbwood{at}iinet.com.au) August 2009.   Dual licensed under the GPL (http://dev.jquery.com/browser/trunk/jquery/GPL-LICENSE.txt) and    MIT (http://dev.jquery.com/browser/trunk/jquery/MIT-LICENSE.txt) licenses.    Please attribute the author if you use it. */(function($){function IslamicCalendar(a){this.local=this.regional[a||'']||this.regional['']}IslamicCalendar.prototype=new $.calendars.baseCalendar;$.extend(IslamicCalendar.prototype,{name:'Islamic',jdEpoch:1948439.5,daysPerMonth:[30,29,30,29,30,29,30,29,30,29,30,29],hasYearZero:false,minMonth:1,firstMonth:1,minDay:1,regional:{'':{name:'Islamic',epochs:['BH','AH'],monthNames:['Muharram','Safar','Rabi\' al-awwal','Rabi\' al-thani','Jumada al-awwal','Jumada al-thani','Rajab','Sha\'aban','Ramadan','Shawwal','Dhu al-Qi\'dah','Dhu al-Hijjah'],monthNamesShort:['Muh','Saf','Rab1','Rab2','Jum1','Jum2','Raj','Sha\'','Ram','Shaw','DhuQ','DhuH'],dayNames:['Yawm al-ahad','Yawm al-ithnayn','Yawm ath-thulaathaa\'','Yawm al-arbi\'aa\'','Yawm al-khamīs','Yawm al-jum\'a','Yawm as-sabt'],dayNamesShort:['Aha','Ith','Thu','Arb','Kha','Jum','Sab'],dayNamesMin:['Ah','It','Th','Ar','Kh','Ju','Sa'],dateFormat:'yyyy/mm/dd',firstDay:6,isRTL:false}},leapYear:function(a){var b=this._validate(a,this.minMonth,this.minDay,$.calendars.local.invalidYear);return(b.year()*11+14)%30<11},weekOfYear:function(a,b,c){var d=this.newDate(a,b,c);d.add(-d.dayOfWeek(),'d');return Math.floor((d.dayOfYear()-1)/7)+1},daysInYear:function(a){return(this.leapYear(a)?355:354)},daysInMonth:function(a,b){var c=this._validate(a,b,this.minDay,$.calendars.local.invalidMonth);return this.daysPerMonth[c.month()-1]+(c.month()==12&&this.leapYear(c.year())?1:0)},weekDay:function(a,b,c){return this.dayOfWeek(a,b,c)!=5},toJD:function(a,b,c){var d=this._validate(a,b,c,$.calendars.local.invalidDate);a=d.year();b=d.month();c=d.day();a=(a<=0?a+1:a);return c+Math.ceil(29.5*(b-1))+(a-1)*354+Math.floor((3+(11*a))/30)+this.jdEpoch-1},fromJD:function(a){a=Math.floor(a)+0.5;var b=Math.floor((30*(a-this.jdEpoch)+10646)/10631);b=(b<=0?b-1:b);var c=Math.min(12,Math.ceil((a-29-this.toJD(b,1,1))/29.5)+1);var d=a-this.toJD(b,c,1)+1;return this.newDate(b,c,d)}});$.calendars.calendars.islamic=IslamicCalendar})(jQuery);/*Written by Keith Wood (kbwood{at}iinet.com.au) August 2009.*/(function($) {$.calendars.calendars.islamic.prototype.regional['ar'] = {name: 'Islamic',epochs: ['BAM', 'AM'],monthNames: ['محرّم', 'صفر', 'ربيع الأول', 'ربيع الآخر أو ربيع الثاني', 'جمادى الاول', 'جمادى الآخر أو جمادى الثاني','رجب', 'شعبان', 'رمضان', 'شوّال', 'ذو القعدة', 'ذو الحجة'],monthNamesShort: ['محرّم', 'صفر', 'ربيع الأول', 'ربيع الآخر أو ربيع الثاني', 'جمادى الاول', 'جمادى الآخر أو جمادى الثاني','رجب', 'شعبان', 'رمضان', 'شوّال', 'ذو القعدة', 'ذو الحجة'],dayNames: ['يوم الأحد', 'يوم الإثنين', 'يوم الثلاثاء', 'يوم الأربعاء', 'يوم الخميس', 'يوم الجمعة', 'يوم السبت'],dayNamesShort: ['يوم الأحد', 'يوم الإثنين', 'يوم الثلاثاء', 'يوم الأربعاء', 'يوم الخميس', 'يوم الجمعة', 'يوم السبت'],dayNamesMin: ['يوم الأحد', 'يوم الإثنين', 'يوم الثلاثاء', 'يوم الأربعاء', 'يوم الخميس', 'يوم الجمعة', 'يوم السبت'],dateFormat: 'yyyy/mm/dd',firstDay: 6,isRTL: true};})(jQuery);/*Khaled Al Horani -- خالد الحوراني -- koko.dw@gmail.com */(function($) {$.calendars.picker.regional['ar'] = {renderer: $.calendars.picker.defaultRenderer,prevText: '&#x3c;السابق', prevStatus: 'عرض الشهر السابق',prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: '',nextText: 'التالي&#x3e;', nextStatus: 'عرض الشهر القادم',nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: '',currentText: 'اليوم', currentStatus: 'عرض الشهر الحالي',todayText: 'اليوم', todayStatus: 'عرض الشهر الحالي',clearText: 'مسح', clearStatus: 'امسح التاريخ الحالي',closeText: 'إغلاق', closeStatus: 'إغلاق بدون حفظ',yearStatus: 'عرض سنة آخرى', monthStatus: 'عرض شهر آخر',weekText: 'أسبوع', weekStatus: 'أسبوع السنة',dayStatus: 'اختر D, M d', defaultStatus: 'اختر يوم',isRTL: true};$.calendars.picker.setDefaults($.calendars.picker.regional['ar']);})(jQuery);