// definition:
// http://en.wikipedia.org/wiki/Pivot_table
// dependencies:
// jquery core 1.3.2+
// jquery ui 1.7.2+
// jquery cookie plugin 1.0+



if (!Array.prototype.forEach){
	Array.prototype.forEach = function(fun){
		var len = this.length;
		if (typeof fun != "function") {
			throw new TypeError();
		}

		var thisp = arguments[1];
		for (var i = 0; i < len; i++){
			if (i in this){
				fun.call(thisp, this[i], i, this);
			}
		}
	};
}

if (!Array.prototype.map){
	Array.prototype.map = function(fun){
		var len = this.length;
		if (typeof fun != "function"){
			throw new TypeError();
		}

		var res = new Array(len);
		var thisp = arguments[1];
		for (var i = 0; i < len; i++){
			if (i in this){
				res[i] = fun.call(thisp, this[i], i, this);
			}
		}

		return res;
	};
}

if (!Array.prototype.reduce){
	Array.prototype.reduce = function(fun /*, initial*/){
		var len = this.length >>> 0;
		if (typeof fun != "function"){
			throw new TypeError();
		}

		// no value to return if no initial value and an empty array
		if (len == 0 && arguments.length == 1){
			throw new TypeError();
		}

		var i = 0;
		if (arguments.length >= 2){
			var rv = arguments[1];
		} else {
			do {
				if (i in this){
					rv = this[i++];
					break;
				}

				// if array contains no values, no initial value to return
				if (++i >= len) {
					throw new TypeError();
				}
			} while (true);
		}

		for (; i < len; i++){
			if (i in this) {
				rv = fun.call(null, rv, this[i], i, this);
			}
		}

		return rv;
	};
}


if (!Array.prototype.filter){
	Array.prototype.filter = function(fun){
		var len = this.length;
		if (typeof fun != "function"){
			throw new TypeError();
		}

		var res = new Array();
		var thisp = arguments[1];
		for (var i = 0; i < len; i++){
			if (i in this){
				var val = this[i]; // in case fun mutates this
				if (fun.call(thisp, val, i, this)){
					res.push(val);
				}
			}
		}

		return res;
	};
}

if (!Array.prototype.some){
	Array.prototype.some = function(fun){
		var len = this.length;
		if (typeof fun != "function"){
			throw new TypeError();
		}
		
		var thisp = arguments[1];
		for (var i = 0; i < len; i++){
			if (i in this && fun.call(thisp, this[i], i, this)) {
				return true;
			}
		}
		return false;
	};
}

pivoter = function() {
    this.pivotViewCount++;
    this.root = '/gringlobal';
    this.url = '~/PivotViewHelper.aspx?';
    this.groupByColOrdinal = -1;
    this.expandByColOrdinal = -1;
    this.selector = null;
    this.aggregate = null;
    this.pageIndex = 0;
    this.pageCount = 1;
    this.pageSize = 2000000000;

    this.allowPaging = true;
    this.allowExporting = true;

    this.allowCookies = true;

    this.allowGrouping = false;
    this.allowSorting = true;
    this.allowFiltering = true;
    this.allowPivoting = true;

    this.allowColumnMovement = true;
    this.allowRowHiding = true;
    this.allowColumnHiding = true;

    this.allowFilteringAutoComplete = true;

    this.sortChanged = false;
    this.filterChanged = false;
    this.groupChanged = false;

    this.rowHeight = 100;

    this.rawData = null;
    this.groupedData = null;
    this.pivotedData = null;

    this.emptyDataMessage = "No data found matching your criteria.";

    this.languageID = 1;


    $(document).click(function(event) {
        if (event && event.target) {

            var tgt = $(event.target);

            if (event.target.tagName) {
                var tag = event.target.tagName.toLowerCase();
                if (tag == 'a' || tag == 'img' || tag == 'input' || tag == 'button' || tag == 'textarea' || tag == 'label') {

                    // don't hide popups if they clicked a control
                    // however, if they clicked a different popup anchor, let it hide
                    if (!tgt.hasClass("popupAnchor") && tgt.parents(".popupAnchor").length == 0) {
                        // do NOT return false, we want the bubbling to happen...
                        return;
                    }
                }
            }

            // if they clicked somewhere inside a popup, we don't want to hide all popups...
            if (tgt.parents(".popup").length > 0) {
                return false;
            }
            if (tgt.parents(".pivotPopup").length > 0) {
                return false;
            }

        }

        pivotView.hideAllPopups();

    }).keypress(function(e) {
        var key = (e.keyCode || e.which);
        if (key == 27) {
            pivotView.hideAllPopups();
        }
        // TODO: add more key handlers here (think exporting, saving, etc)
    });
    return this;
}

pivoter.prototype = {
    pivotViewCount: 0,

    rememberPrimaryKeys: function(rowProp, dataOrMode) {
        var pks = this.getPrimaryKeyValues(rowProp);
        var pklist = pks.join(',');
        $("input[name='pivotHiddenPKList']").val(pklist);
        return pks;
    },

    // rowProp can be: 'checked', 'unchecked', 'highlighted', 'unhighlighted', or 'all'.  All other values are ignored.
    getPrimaryKeyValues: function(rowProp, dataOrMode) {
        var all = [];
        var data = this.getDataSource(dataOrMode);
        if (data && data.rows && data.rows.length > 0) {
            var pkOrdinal = this.getOrdinal(data.primaryKeyName, data);
            if (pkOrdinal < 0) {
                alert('Ordinal for primary key column = "' + data.primaryKeyName + '" could not be found.');
            } else {
                var obj = this;
                for (var i = 0; i < data.rows.length; i++) {
                    var row = data.rows[i];
                    var rawRows = obj.getRawRows(row);
                    for (var j = 0; j < rawRows.length; j++) {
                        var rawRow = rawRows[j];
                        switch (rowProp) {
                            case "checked":
                                if (rawRow.isChecked) {
                                    all.push(rawRow.dbID);
                                }
                                break;
                            case "unchecked":
                                if (!rawRow.isChecked) {
                                    all.push(rawRow.dbID);
                                }
                                break;
                            case "highlighted":
                                if (rawRow.isHighLighted) {
                                    all.push(rawRow.dbID);
                                }
                                break;
                            case "unhighlighted":
                                if (!rawRow.isHighLighted) {
                                    all.push(rawRow.dbID);
                                }
                                break;
                            case "all":
                                all.push(rawRow.dbID);
                                break;
                            default:
                                alert("Bad value '" + rowProp + "' passed into pivotView.getPrimaryKeyValues for rowProp. Must be 'checked', 'unchecked', 'highlighted', 'unhighlighted', or 'all'.");
                                return null;
                        }
                    }
                }
            }
        }

        var ret = this.unique(all);
        return ret;

    },

    getRawRows: function(row) {
        var ret = [];

        var rawData = this.getDataSource('raw');
        if (this.isDef(row.fakeIDs)) {
            for (var i = 0; i < row.fakeIDs.length; i++) {
                // we must match on the fakeID property because sorting will
                // change the array offset.
                var fakeID = row.fakeIDs[i];
                var origRowIndex = this.arrayIndexOf(rawData.rows, 'fakeID', fakeID);
                if (origRowIndex > -1) {
                    ret.push(rawData.rows[origRowIndex]);
                }
            }
        } else if (this.isDef(row.fakeID)) {
            var origRowIndex = this.arrayIndexOf(rawData.rows, 'fakeID', row.fakeID);
            if (origRowIndex > -1) {
                ret.push(rawData.rows[origRowIndex]);
            }
        } else {
            this.debug(row, 'invalid value for row');
        }
        return ret;
    },

    savePreference: function(prefName, prefValue) {
        if (this.allowCookies) {

            // TODO: instead of kicking out a bazillion cookies, combine all into 1 big cookie.

            // all cookies expire 10 years (3,650 days) from when they were last written
            var nm = location.pathname + '__' + prefName;
            $.cookie(nm, prefValue, { expires: 3650 });

            //console.log('saving cookie ' + nm + '=' + prefValue);
        }
    },

    loadPreference: function(prefname, defaultValue) {
        if (!this.allowCookies) {
            return defaultValue;
        } else {
            var val = $.cookie(location.pathname + '__' + prefname);
            if (!this.isDef(val)) {
                val = defaultValue;
            }
            return val;
        }
    },


    saveLabel: function(allList) {
        //allList = 'lblSelect=select;lblHighlight=highlight;lblFrom=from;lblTo=to;lblExport=Export***';
        if (this.allowCookies) {

            var langid = allList.substring(0, 1);
            allList = allList.substring(1);
            var listArray = allList.split(";");

            for (var i = 0; i < listArray.length; i++) {
                var one = listArray[i].toString();
                var lblName = one.split('=')[0];
                var lblValue = one.split('=')[1];

                var nm = location.pathname + '__' + langid + '_' + lblName;
                $.cookie(nm, lblValue, { expires: 3650 });
            }
        }
    },


    loadLabel: function(lblname, defaultValue) {
        if (!this.allowCookies) {
            return defaultValue;
        } else {
            var val = $.cookie(location.pathname + '__' + this['languageID'] + '_' + lblname);
            if (!this.isDef(val)) {
                val = defaultValue;
            }
            return val;
        }
    },


    changeRowHeight: function(newHeight) {
        this.rowHeight = newHeight;
        this.savePreference('row_height', this.rowHeight);
        this.pivotOptionsVisible = true;


        return this;
    },

    toggleAllCheckBoxes: function(chk, mode) {

        var data = this.getDataSource();
        if (data.rows) {
            for (var i = 0; i < data.rows.length; i++) {
                var row = data.rows[i];
                var checked = null;
                var nm = "input[name='pivotViewCheckBox" + i + "']";
                if (mode == 'off') {
                    this.toggleCheckBox(false, i);
                    $(nm).attr('checked', false);
                } else if (mode == 'on') {
                    this.toggleCheckBox(true, i);
                    $(nm).attr('checked', true);
                } else if (mode == 'inverse') {
                    checked = row.isChecked ? "" : "checked";
                    this.toggleCheckBox(checked, i);
                    $(nm).attr('checked', checked);
                } else if (mode == 'hilite') {
                    checked = row.isHighlighted ? "checked" : "";
                    this.toggleCheckBox(checked, i);
                    $(nm).attr('checked', checked);
                } else {
                    this.toggleCheckBox(chk, i);
                    $(nm).attr('checked', chk);
                }
            }
        }

        var all = "input[name='pivotViewCheckBoxAll']";
        if (mode == 'off') {
            $(all).attr('checked', false);
            this.allChecked = false;
        } else if (mode == 'on') {
            $(all).attr('checked', true);
            this.allChecked = true;
        } else {
            $(all).attr('checked', chk);
            this.allChecked = chk;
        }

        return this;

    },

    toggleCheckBox: function(chk, i) {
        var row = this.getDataSource().rows[i];
        row.isChecked = chk;
        if (this.isDef(row.fakeIDs)) {
            // this is not a raw row. we need to mark those as well.
            var rawRows = this.getRawRows(row);
            for (var i = 0; i < rawRows.length; i++) {
                rawRows[i].isChecked = chk;
            }
        }

        return this;

    },

    clearCaches: function() {
        this.groupedData = null;
        this.groupChanged = true;

        this.pivotedData = null;
        this.pivotChanged = true;

        return this;
    },

    hideRows: function(opt, dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        var obj = this;
        if (data && data.rows && data.rows.length > 0) {
            if (opt == 'checked') {
                data.rows.forEach(function(row, i) {
                    if (row.isChecked) {
                        // hide the row, then uncheck it
                        obj.toggleRow(i, data);
                        obj.toggleCheckBox(false, i);
                    }
                });
            } else {
                // TODO: implement more opt values...


                alert('bad opt passed to hideRows: ' + opt);

            }
        }

        return this;
    },

    toggleRow: function(idx, dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        if (data.rows && data.rows.length > idx && idx > -1) {
            var row = data.rows[idx];

            if (!row.hideMask) {
                row.hideMask |= 1;
                data.hiddenRowsCount++;
            } else {
                row.hideMask = 0;
                data.hiddenRowsCount--;
            }

            var rawRows = this.getRawRows(row);
            for (var i = 0; i < rawRows.length; i++) {
                var rawRow = rawRows[i];
                if (!rawRow.hideMask) {
                    rawRow.hideMask |= 1;
                    this.rawData.hiddenRowsCount++;
                } else {
                    rawRow.hideMask = 0;
                    this.rawData.hiddenRowsCount--;
                }
            }
        }
        return this;
    },

    toggleExporterTextArea: function() {
        $("textarea.exporterTextArea").each(function() {
            $(this).toggle();
            if ($(this).is(":visible")) {
                this.focus();
                this.select();
            }
        });

        return this;
    },

    refreshExporterTextArea: function(dataOrMode) {

        var obj = this;

        this.exportFormat = $("select[name='exportFormat']").val();
        $("textarea.exporterTextArea").each(function() {
            var rows = obj.calcExportRows(dataOrMode);
            this.value = obj.exportData('tab', true, rows, dataOrMode);
            var html = '';
            if (rows.length == 0) {
                html = 'No ' + obj.loadLabel('lblRowsExport', 'rows to export');
            } else if (rows.length == 1) {
                html = '1 ' + obj.loadLabel('lblRowsExport', 'rows to export');
            } else {
                //html = rows.length + ' rows to export';
                html = rows.length + ' ' + obj.loadLabel('lblRowsExport', 'rows to export');
            }
            $("div.exporterRowCount").html(html);
        });

        return this;
    },

    toggleExporter: function(el) {

        $(".exporter").slideToggle("fast");

        if (!$(".exporter").is(":hidden")) {
            this.refreshExporterTextArea();
        }

        return this;
    },

    getCsvValue: function(val1, val2, val3) {
        var val = val1;
        if (!val || val.length == 0) {
            val = val2;
        }
        if (!val || val.length == 0) {
            val = val3;
        }

        if (val) {
            val = val.toString().replace(/<br \/>/gi, ", ");
            val = val.replace(/<[^>]+>/gi, '');

            if (val.length > 0) {
                if (val.indexOf('"') > -1 || val.indexOf(',') > -1 || val.indexOf(' ') > -1 || val.indexOf('\t') > -1 || val.indexOf("\n") > -1) {
                    val = '"' + val.replace('"', '""') + '"';
                }
            }
            return val;
        } else {
            return '';
        }
    },

    getTabbedValue: function(val1, val2, val3) {
        var val = val1;
        if (!val || val.length == 0) {
            val = val2;
        }
        if (!val || val.length == 0) {
            val = val3;
        }

        if (val) {

            val = val.toString().replace(/<br \/>/gi, ", ");
            val = val.replace(/<[^>]+>/gi, '');

            if (val.length > 0) {
                if (val.indexOf('"') > -1 || val.indexOf('\t') > -1 || val.indexOf('\n') > -1) {
                    val = '"' + val.replace('"', '""') + '"';
                }
            }
            return val;
        } else {
            return '';
        }
    },

    exportDataToFile: function(fmt, dataOrMode) {

        fmt = fmt || $("select[name='exportFormat']").val();

        var data = this.exportData(fmt, false, null, dataOrMode);
        if (data) {
            var win = window.open('');
            this.bounceData(fmt, data, function(json) {
                if (fmt == 'csv') {
                    //					window.open(json.url);
                    location.href = json.url;
                    win.close();
                } else {
                    win.location.href = json.url;
                }
            });
            //		
            //			var win = window.open('');
            //			var doc = win.document;
            //			doc.write("<html><body >");
            //			doc.write("<form name='form0' action='" + this.resolveUrl(this.url) + "' method='POST'>");
            //			doc.write("<input type='hidden' name='action' value='export' />");
            //			doc.write("<input type='hidden' name='format' value='" + fmt + "' />");
            //			doc.write("<input type='hidden' name='data' value='" + data.replace("'", "&apos;") + "' />");
            //			doc.write("<div disabled='disabled'><input type='submit' name='btnExport' value='Exporting data...' /></div>");
            //			doc.write("</form></body></html>");
            //			doc.close();
        }

        return this;

    },

    resolveUrl: function(url) {
        return url.replace('~/', this.root + '/').replace('//', '/');
    },

    doExport: function(rows, fn, sep, dataOrMode) {
        var output = '';
        // write header row
        var rowdata = '';
        var isfirstCol = true;
        var cols = this.getColumns(dataOrMode);
        cols.forEach(function(col, idx) {
            if (!col.isHidden) {
                if (!isfirstCol) {
                    rowdata += sep
                }
                rowdata += fn(col.caption, col.columnName, col.ordinal);
                isfirstCol = false;
            }
        });
        output += rowdata + '\r\n';

        // write data row
        rows.forEach(function(row, i) {
            if (row && !row.hideMask) {
                var rowdata = '';
                var isfirstCol = true;
                var val = null;
                cols.forEach(function(col, j) {
                    if (!col.isHidden) {
                        val = row.values[col.ordinal];
                        if (!isfirstCol) {
                            rowdata += sep;
                        }
                        rowdata += fn(val);
                        isfirstCol = false;
                    }
                });
            }
            output += rowdata + '\r\n';
        });
        return output;
    },

    calcExportRows: function(dataOrMode) {
        var filter = $("select[name='exportRows']").val();

        var src = this.getDataSource(dataOrMode);
        var rows = null;
        switch (filter) {
            case 'all':
                this.exportRows = 'all';
                rows = src.rows.filter(function(row) {
                    return !row.hideMask;
                });
                break;
            case 'hilite':
                this.exportRows = 'hilite';
                rows = src.rows.filter(function(row) {
                    return row.isHighlighted && !row.hideMask;
                });
                break;
            case 'checked':
                this.exportRows = 'checked';
                rows = src.rows.filter(function(row) {
                    return row.isChecked && !row.hideMask;
                });
                break;
            case 'page':
                this.exportRows = 'page';
                var start = this.pageIndex * this.pageSize;
                rows = src.rows.slice(start, start + this.pageSize).filter(function(row) {
                    return !row.hideMask;
                });
                break;
        }

        return rows;
    },

    exportData: function(fmt, doNotPrompt, rows, dataOrMode) {
        fmt = fmt || 'csv';

        rows = rows || this.calcExportRows(dataOrMode);

        if (rows.length == 0 && !doNotPrompt) {
            if (!confirm('This export will result in no rows.\nDo you want to continue?')) {
                return null;
            }
        }

        var data = '';
        if (fmt == 'csv') {
            data = this.doExport(rows, this.getCsvValue, ',', dataOrMode);
        } else if (fmt == 'tab') {
            data = this.doExport(rows, this.getTabbedValue, '\t', dataOrMode);
        }
        return data;

    },

    hideAllPopups: function() {

        this.pivotOptionsVisible = false;
        $(".pivotPopup").hide();
        $(".popup").hide();
        $(".exporterTextArea").hide(); // rendering text area with large data sets takes awhile, so be sure to hide it so the next time they show the exporter popup it is already hidden
        $(".ac_results").hide();
        return;

    },


    removeAllFilters: function(dataOrMode) {
        var pvt = this;
        this.getColumns(dataOrMode).forEach(function(col, idx) {
            // col.filterText = null;  // leave the filterText alone so it is remembered if they re-enable it
            col.isFiltered = false;
            pvt.savePreference('col_isFiltered_' + col.columnName, col.isFiltered);
        });
        this.pivotOptionsVisible = true;
        return this;
    },

    removeAllSorts: function(dataOrMode) {
        var pvt = this;
        this.getColumns(dataOrMode).forEach(function(col, idx) {
            col.sortIndex = -1;
            col.sortDescending = false;
            col.sortDescendingChanged = true;
            pvt.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
            pvt.savePreference('col_sortDescending_' + col.columnName, col.sortDescending);
        });
        this.pivotOptionsVisible = true;
        return this;
    },
    hidePivotOptions: function() {
        $(".pivotOptions").slideToggle("fast"); //.hide("fast");
        this.pivotOptionsVisible = false;
        return this;
    },

    applySortChanges: function(dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        var pvt = this;

        $(".sortSource li").each(function() {
            var offset = pvt.getOffset($(this).text(), dataOrMode);
            if (offset > -1) {
                var col = data.columns[offset];
                if (!this.sortChanged) {
                    this.sortChanged = col.sortIndex != -1;
                }
                col.sortIndex = -1;
                var desc = $("img", $(this)).attr('src').indexOf("up.gif") > -1;
                if (desc != col.sortDescending) {
                    col.sortDescendingChanged = true;
                } else {
                    col.sortDescendingChanged = false;
                }
                col.sortDescending = desc;
                pvt.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
                pvt.savePreference('col_sortDescending_' + col.columnName, col.sortDescending);
            }
        });

        var sortIndex = 0;
        $(".sortTarget li").each(function() {
            var offset = pvt.getOffset($(this).text(), dataOrMode);
            //alert($(this).text() + "\n" + offset);
            if (offset > -1) {
                var col = data.columns[offset];
                if (!this.sortChanged) {
                    this.sortChanged = col.sortIndex != sortIndex;
                }
                col.sortIndex = sortIndex++;
                var desc = $("img", $(this)).attr('src').indexOf("up.gif") > -1;
                if (desc != col.sortDescending) {
                    col.sortDescendingChanged = true;
                } else {
                    col.sortDescendingChanged = false;
                }
                col.sortDescending = desc;
                pvt.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
                pvt.savePreference('col_sortDescending_' + col.columnName, col.sortDescending);
            }
        });

        // we've tweaked copied over the existing columns, but 
        // in the order specified by the sortable control.
        // just reassign the columns property to the newColumns so
        // sorting is taken care of
        this.pivotOptionsVisible = true;
        return this;

    },

    changeSortIndex: function(ordinal, newIndex, dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        var tempIndex = null;
        var tempOffset = null;
        var pvt = this;
        for (var i = 0; i < data.columns.length; i++) {
            var col = data.columns[i];
            if (col.ordinal == ordinal) {
                tempIndex = col.sortIndex;
                col.sortIndex = newIndex;
                this.sortChanged = true;
                pvt.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
            }
        }
        return this;
    },

    changeFilterText: function(colDef, txt, showPrefsPopup, dataOrMode) {
        var col = this.getColumn(colDef, dataOrMode);
        if (col) {
            if (col.filterText != txt || !col.isFiltered) {
                this.filterChanged = true;
                col.filterChanged = true;
            }

            col.filterText = txt;

            if (txt && txt.length > 0) {
                col.isFiltered = true;

                if (!col.filterMask) {
                    col.filterMask = Math.pow(2, (col.ordinal + 1));
                }

            } else {
                col.isFiltered = false;
            }
            col.prefsPopupVisible = showPrefsPopup;

            this.savePreference('col_filterText_' + col.columnName, col.filterText);
            this.savePreference('col_isFiltered_' + col.columnName, col.isFiltered);
            this.savePreference('col_filterMask_' + col.columnName, col.filterMask);
        }
        return this;
    },

    toggleFilter: function(colDef, txt, showPivotOptions, showPrefsPopup, dataOrMode) {
        var col = this.getColumn(colDef, dataOrMode);
        if (col) {
            col.isFiltered = !col.isFiltered;
            col.filterText = txt;
            col.filterChanged = true;
            this.filterChanged = true;
            this.savePreference('col_isFiltered_' + col.columnName, col.isFiltered);
            this.savePreference('col_filterText_' + col.columnName, col.filterText);
        }
        this.pivotOptionsVisible = showPivotOptions;
        col.prefsPopupVisible = showPrefsPopup;
        return this;
    },

    toggleSort: function(colDef, showPivotOptions, dataOrMode) {
        var col = this.getColumn(colDef, dataOrMode);
        if (col) {
            if (col.sortIndex > -1) {
                this.sortChanged = col.sortIndex != -1;

                var oldSortIndex = col.sortIndex;
                col.sortIndex = -1;

                this.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);

                // move everybody who is sorted down one...
                var cols = this.getColumns(dataOrMode);
                for (var i = 0; i < cols.length; i++) {
                    if (cols[i].sortIndex >= oldSortIndex) {
                        cols[i].sortIndex--;
                        this.savePreference('col_sortIndex_' + cols[i].columnName, cols[i].sortIndex);
                    }
                }

            } else {
                // move everybody who is sorted, but above us, up one...
                var highest = 0;
                var cols = this.getColumns(dataOrMode);
                for (var i = 0; i < cols.length; i++) {
                    if (cols[i].sortIndex >= highest) {
                        highest++;
                    }
                }
                this.sortChanged = col.sortIndex != highest;
                col.sortIndex = highest;
                this.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
            }
        } else {
            alert('could not toggle sort on column ' + colDef + ' because it was not found.');
        }
        this.pivotOptionsVisible = showPivotOptions;
        return this;
    },

    toggleSortDir: function(colDef, showPivotOptions, dataOrMode) {
        var col = this.getColumn(colDef, dataOrMode);
        col.sortDescending = !col.sortDescending;
        this.savePreference('col_sortDescending_' + col.columnName, col.sortDescending);
        col.sortDescendingChanged = true;
        this.sortChanged = true;
        this.pivotOptionsVisible = showPivotOptions;
        return this;
    },

    prioritizeSortColumn: function(colDef, dataOrMode) {

        // essentially this will make the given colDef the most important of the sorted columns (sortIndex = 0)
        // will properly reprioritize all the other columns

        var cols = this.getColumns(dataOrMode);
        var priorityCol = this.getColumn(colDef, dataOrMode);
        if (priorityCol) {
            if (priorityCol.sortIndex != 0) {

                this.sortChanged = true;

                priorityCol.autoSorted = true;
                priorityCol.sortIndex = 0;
                priorityCol.sortDescending = false;
                priorityCol.sortDescendingChanged = true;

                this.savePreference('col_autoSorted_' + priorityCol.columnName, priorityCol.autoSorted);
                this.savePreference('col_sortDescending_' + priorityCol.columnName, priorityCol.sortDescending);
                this.savePreference('col_sortIndex_' + priorityCol.columnName, priorityCol.sortIndex);

                var nextIndex = 1;
                var sortedCols = this.arraySortBy(cols, 'sortIndex', false, true, 'number');
                for (var i = 0; i < sortedCols.length; i++) {
                    if (sortedCols[i].sortIndex > -1) {
                        if (priorityCol != sortedCols[i]) {
                            sortedCols[i].sortIndex = nextIndex++;
                            this.savePreference('col_sortIndex_' + sortedCols[i].columnName, sortedCols[i].sortIndex);
                        }
                    }
                }
            }
        }
        return this;
    },

    rowClick: function(el, offset, dataOrMode, event) {

        if (!event) {
            alert('no event given');
            return this;
        }

        var tag = event.target ? event.target.tagName : event.srcElement ? event.srcElement.tagName : '';
        tag = tag.toLowerCase();
        //		var db = '';
        //		for(var prop in event){
        //			db += prop + '=' + event[prop] + '\n';
        //		}
        //		alert('event=' + db + '\n\ntag=' + tag);
        if (tag == 'a' || tag == 'input' || tag == 'img') {
            // do not toggle highlighting on clickable items
            return this;
        }

        $(el).toggleClass('hilite');
        var row = this.getDataSource(dataOrMode).rows[offset];
        var hilited = $(el).hasClass('hilite');
        row.isHighlighted = hilited;

        // make sure the raw rows are in sync with the current data source
        var rawRows = this.getRawRows(row);
        for (var i = 0; i < rawRows.length; i++) {
            rawRows[i].isHighlighted = hilited;
        }

        if ($(".exporter").is(":visible")) {
            this.refreshExporterTextArea(dataOrMode);
        }


        return this;
    },

    changePageSize: function(newPageSize, dataOrMode) {

        // determine the new page. i.e. keep same item that was at the top as close to the top as possible
        var topRowIndex = this.pageSize * this.pageIndex;
        this.pageSize = parseInt(newPageSize, 10);
        var currentpi = parseInt(topRowIndex / this.pageSize, 10);
        if (currentpi < 0)
            currentpi = 0;
        this.pageIndex = currentpi;

        this.savePreference('page_size', this.pageSize);
        this.savePreference('page_index', this.pageIndex);

        var data = this.getDataSource(dataOrMode);
        if (data) {
            this.pageCount = (data.rows.length - data.hiddenRowsCount) / this.pageSize;
        }

        return this;
    },

    changePageTo: function(newPage) {
        if (newPage == 0) {
            this.pageIndex = 0;
        } else if (newPage == -1) {
            this.pageIndex--;
        } else if (newPage == -2) {
            this.pageIndex++;
        } else if (newPage == -3) {
            this.pageIndex = this.pageCount - 1;
        } else {
            this.pageIndex = parseInt(newPage, 10);
        }

        if (this.pageIndex < 0) {
            this.pageIndex = 0;
        } else if (this.pageIndex >= this.pageCount) {
            this.pageIndex = this.pageCount - 1;
        }

        this.savePreference('page_index', this.pageIndex);
        return this;

    },

    toggleAllColumns: function(action, dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        for (var i = 0; i < data.columns.length; i++) {
            if (action == 'hide') {
                data.columns[i].isHidden = true;
            } else if (action == 'show') {
                data.columns[i].isHidden = false;
            } else if (action == 'inverse') {
                data.columns[i].isHidden = !data.columns[i].isHidden;
            }
            this.savePreference('col_isHidden_' + data.columns[i].columnName, data.columns[i].isHidden);
        }
        this.columnEditorVisible = true;
        return this;
    },

    getDataSource: function(dataOrMode) {

        if (this.isDef(dataOrMode)) {
            if (this.isObject(dataOrMode)) {
                // they specified the data. just return it.
                return dataOrMode;
            } else if (this.isString(dataOrMode)) {
                switch (dataOrMode) {
                    case 'raw':
                        return this.rawData;
                    case 'group':
                    case 'grouped':
                        return this.groupedData;
                    case 'pivot':
                    case 'pivoted':
                        return this.pivotedData;
                    default:
                        if (!dataOrMode) {
                            return this.pivotedData || this.groupedData || this.rawData;
                        } else {
                            alert('possibly bad dataOrMode=' + dataOrMode + ' passed to getDataSource() from ' + this.getCallStack(3));
                        }
                }
            }
        }

        if (this.groupedData != null) {
            return this.groupedData;
        } else {
            // they didn't give us the data.
            // just return the raw data.
            return this.rawData;
        }

        dataOrMode = dataOrMode || this.dataOrMode;
    },

    getCallStack: function(max) {
        max = max || 8;
        // bubble up 2 calls to get the one just before the one we're called from
        var prev, next;
        var first = arguments.callee.caller;
        next = first.arguments.callee.caller;
        var output = '';
        var i = 0;
        do {
            prev = next;
            //alert(prev.toString());
            output += "\n" + prev.toString();
            next = next.arguments.callee.caller;
            i++;
        } while (i < max);
        return output;

    },

    resetColumnChanges: function(dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        var count = 0;
        var newColumns = [];
        var pvt = this;
        $(".columnEditor ul.sortable li").each(function() {
            var chk = $("input:checkbox", this);
            var offset = pvt.getOffset(chk.parent().text(), dataOrMode);
            if (offset > -1) {
                var col = data.columns[offset];
                if (col.columnName == data.primaryKeyName) {
                    col.isHidden = col.ordinal != this.groupByColOrdinal;
                } else {
                    col.isHidden = false;
                }
                this.savePreference('col_isHidden_' + col.columnName, col.isHidden);
                // resetting means putting them in order by their ordinal values
                newColumns[col.ordinal] = col;
            }
        });

        // we've tweaked copied over the existing columns, but 
        // in the order specified by the sortable control.
        // just reassign the columns property to the newColumns so
        // sorting is taken care of
        data.columns = newColumns;
        this.columnEditorVisible = false;
        this.changeGroupByColumn(-1);
        return this;
    },

    applyColumnChanges: function(dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        var count = 0;
        var newColumns = [];
        var pvt = this;
        $(".columnEditor ul.sortable li").each(function() {
            var chk = $("input:checkbox", this);
            var offset = pvt.getOffset(chk.parent().text(), dataOrMode);
            if (offset > -1) {
                data.columns[offset].isHidden = !chk.is(":checked");
            }
            pvt.savePreference('col_isHidden_' + data.columns[offset].columnName, data.columns[offset].isHidden);
            newColumns.push(data.columns[offset]);
        });

        // we've tweaked copied over the existing columns, but 
        // in the order specified by the sortable control.
        // just reassign the columns property to the newColumns so
        // sorting is taken care of
        data.columns = newColumns;
        this.columnEditorVisible = true;
        return this;
    },

    showAllNonFilteredRows: function(dataOrMode) {
        var data = this.getDataSource(dataOrMode);
        data.rows.forEach(function(row) {
            //alert(row.hideMask);
            if (row.hideMask & 0x01 == 1) {
                // row is marked as hidden by user.
                // it may also be marked as hidden by a filter.
                // only unset the hidden by user flag (0x01)
                row.hideMask &= 0xFFFFFFFE;

                // if no hide flags exist, it is now unhidden!  yay.
                if (row.hideMask == 0) {
                    data.hiddenRowsCount--;
                }
            } else {
                // row is hidden by a filter, do not show
            }
        });

        return this;

    },

    showAllColumns: function(dataOrMode) {
        var cols = this.getDataSource(dataOrMode).columns;
        for (var i = 0; i < cols.length; i++) {
            cols[i].isHidden = false;
            this.savePreference('col_isHidden_' + cols[i].columnName, cols[i].isHidden);
        }
        return this;
    },

    toggleColumn: function(colName, dataOrMode) {
        var offset = this.getOffset(colName, dataOrMode);
        if (offset > -1) {
            var col = this.getDataSource(dataOrMode).columns[offset];
            if (!col.isHidden) {
                col.isHidden = true;
                //alert('hiding column ' + col.columnName);
                //				if (!$(".columnRowAdderAnchor")){
                //				} else {
                //					var obj = this;
                //					$(".columnPref.pivotPopup.col" + col.ordinal).effect( 'transfer', { to : '.columnRowAdderAnchor' }, 300, function() { obj.refresh(); } );
                //				}
            } else {
                col.isHidden = false;
                //alert('showing column ' + col.columnName);
            }
            this.savePreference('col_isHidden_' + col.columnName, col.isHidden);
        }
        return this;
    },

    hideColumn: function(colName, dataOrMode) {
        var offset = this.getOffset(colName, dataOrMode);
        if (offset > -1) {
            //console.log('hiding column ' + colName);
            var ds = this.getDataSource(dataOrMode);
            ds.columns[offset].isHidden = true;
            this.savePreference('col_isHidden_' + ds.columns[offset].columnName, ds.columns[offset].isHidden);
            this.refresh();
        }
        return this;
    },

    shiftColumns: function(draggedDef, droppedDef, dataOrMode) {
        var offset1 = this.getOffset(draggedDef, dataOrMode);
        var offset2 = this.getOffset(droppedDef, dataOrMode);

        var data = this.getDataSource();

        //alert('o1=' + offset1 + ', o2=' + offset2);

        if (offset1 < offset2) {
            var temp = data.columns[offset1];
            for (var i = offset1 + 1; i <= offset2; i++) {
                //			alert('col[i-1]=' + data.columns[i-1].caption + ', col[i]=' + data.columns[i].caption);
                data.columns[i - 1] = data.columns[i];
            }
            data.columns[offset2] = temp;
        } else {
            var temp = data.columns[offset1];
            for (var i = offset1; i > offset2; i--) {
                //alert('col[i-1]=' + data.columns[i - 1].caption + ', col[i]=' + data.columns[i].caption);
                data.columns[i] = data.columns[i - 1];
            }
            data.columns[offset2] = temp;
        }


        return this;
    },

    swapColumns: function(colDef1, colDef2, dataOrMode) {
        var col1Offset = this.getOffset(colDef1, dataOrMode);
        var col2Offset = this.getOffset(colDef2, dataOrMode);

        var data = this.getDataSource();
        var temp = data.columns[col1Offset];
        data.columns[col1Offset] = data.columns[col2Offset];
        data.columns[col2Offset] = temp;

        return this;

    },

    getColumnProperty: function(colDef, prop, defaultVal, dataOrMode) {
        var col = this.getColumn(colDef, dataOrMode);
        if (col) {
            if (this.isDef(col[prop])) {
                return col[prop];
            }
        }
        return defaultVal;
    },

    getOrdinal: function(colDef, dataOrMode) {
        return this.getColumnProperty(colDef, 'ordinal', -1, dataOrMode);
    },

    getColumns: function(dataOrMode) {
        return this.getDataSource(dataOrMode).columns;
    },

    getColumn: function(colDef, dataOrMode) {
        var offset = this.getOffset(colDef, dataOrMode);
        if (offset > -1) {
            return this.getDataSource(dataOrMode).columns[offset];
        } else {
            return null;
        }
    },

    getOffset: function(colDef, dataOrMode) {
        var iscolDef = false;
        if (isNaN(colDef) && typeof (colDef) == 'string') {
            colDef = $.trim(colDef);
            iscolDef = true;
        }
        var data = this.getDataSource(dataOrMode);
        for (var i = 0; i < data.columns.length; i++) {
            var col = data.columns[i];
            //alert('colDef=' + colDef + ': ' + col.columnName + "?");
            if (colDef == col || iscolDef && (col.columnName.toUpperCase() == colDef.toUpperCase()) || col.caption == colDef || col.ordinal.toString() == colDef) {
                return i;
            }
        }
        return -1;
    },

    initData: function(json, callback, allLabel) {
        this.saveLabel(allLabel);
        this.groupByColOrdinal = -1;
        this.expandByColOrdinal = -1;
        this.rawData = json;

        // the rule of thumb is: never add a property to an array, as someone may need to slice() it
        // and the property will not be copied.  Always add properties to an object (which may contain an array)

        var obj = this;

        if (this.rawData) {


            this.rawData.columns.forEach(function(col, i) {
                // init column properties
                col.sortIndex = -1;
                col.sortDescending = false;
                col.sortDescendingChanged = false;

                col.isFiltered = false;
                col.filterMask = Math.pow(2, (col.ordinal + 1));
                col.filterText = '';

                // make this array so we can store this list of unique values for this column (for autocompleter)
                col.values = [];

            });

            var pkOffset = this.getOffset(this.rawData.primaryKeyName);
            if (pkOffset > -1) {
                if (altKeyCol && altKeyCol == this.rawData.columns[pkOffset]) {
                    // pk field == alt key, meaning user should see it. do not hide pk column.
                } else {
                    // if primary key field is not the same as the alt key column, hide it. (assume user should not see it)
                    this.rawData.columns[pkOffset].isHidden = true;
                }
            }

            var altKeyCol = this.getColumn(this.rawData.alternateKeyName);
            if (altKeyCol) {
                // always default to goruping by the alternate key field (even if allowGrouping is false)
                this.changeGroupByColumn(altKeyCol);
            }

            this.rawData.hiddenRowsCount = 0;
            this.rawData.filteredRowsCount = 0;
            this.rawData.dataOrMode = 'raw';

            this.allChecked = false;

            this.rawData.rows.forEach(function(row, i) {
                // init row properties
                row.hideMask = 0;
                row.isHighlighted = false;
                row.isChecked = false;

                // the following properties allow us
                // to align groupedData.rows and pivotedData.rows with rawData.rows.
                // since they can sort the raw data, we need to store off this as an ID as we can't guarantee offset won't change
                // using this fakeID also lets us re-display it in the original sort order when all sorting is removed
                row.fakeID = i;
                if (pkOffset > -1) {
                    row.dbID = row.values[pkOffset];
                }


                row.values.forEach(function(val, i) {
                    if (obj.isString(val)) {
                        row.values[i] = val.replace('~/', obj.root + '/').replace('//', '/');
                    }
                });

                // don't create fakeIDs array on the rawData, it will never be used.
                // that property will appear only on grouped and pivoted data (as one row in those sets may represent several in the raw data set, but one raw row always == one raw row)
                // row.fakeIDs = [];

                // the following is for autocomplete...
                // it generates a lot of arrays though so skip it if we can
                if (obj.allowFilteringAutoComplete) {
                    obj.rawData.columns.forEach(function(col, j) {
                        // init column values 
                        col.values.push(row.values[j]);
                    });
                }
            });


            if (this.allowFilteringAutoComplete) {
                // now that we've filled all the col.values[] arrays, we need to get only unique values for it...
                this.rawData.columns.forEach(function(col, i) {
                    col.values = obj.unique(col.values);
                });
            }

        }

        // clear out grouped / pivoted data, as their values are now out of date and should be regenerated
        this.groupedData = null;
        this.pivotedData = null;



        var ps = parseInt(this.loadPreference('page_size', -1), 10);
        var groupByCol = this.loadPreference('group_by_col_ordinal', -1);
        var expandByCol = this.loadPreference('expand_by_col_ordinal', -1);
        var rowH = this.loadPreference('row_height', this.rowHeight);
        var agg = this.loadPreference('aggregate', false);
        var pi = parseInt(this.loadPreference('page_index', 0), 10);



        if (callback) {
            callback(this.rawData);
        } else {
            // they didn't explicitly bind anything, do a refresh
            this.refresh();
        }

        var changed = this.loadAllPreferences(this.getDataSource(), ps, groupByCol, expandByCol, rowH, agg, pi);
        if (changed) {
            //console.log('refreshing due to pref change');
            this.refresh();
        }

        return this;
    },

    loadAllPreferences: function(data, ps, groupByCol, expandByCol, rowH, agg, pi) {
        var changed = false;

        var obj = this;

        data.columns.forEach(function(col, i) {
            var c = { colName: col.columnName };
            c.autoSort = obj.loadPreference('col_autoSorted_' + col.columnName, false) == 'true';
            c.sortIndex = parseInt(obj.loadPreference('col_sortIndex_' + col.columnName, -1), 10);
            c.sortDesc = obj.loadPreference('col_sortDescending_' + col.columnName, false) == 'true';
            c.isFiltered = obj.loadPreference('col_isFiltered_' + col.columnName, false) == 'true';
            c.filterText = obj.loadPreference('col_filterText_' + col.columnName, '');
            c.isHidden = obj.loadPreference('col_isHidden_' + col.columnName, false) == 'true';

            //            obj.debug(c);


            if (c.autoSort) {
                col.autoSort = true;
                changed = true;
            }
            if (c.sortIndex > -1) {
                col.sortIndex = c.sortIndex;
                changed = true;
            }
            if (c.sortDesc) {
                col.sortDescending = true;
                changed = true;
            }
            if (c.isHidden) {
                col.isHidden = true;
                changed = true;
            }
            if (c.isFiltered) {
                col.isFiltered = true;
                col.filterText = c.filterText;
                changed = true;
            }
        });

        if (ps != this.pageSize && ps > 0) {
            this.changePageSize(ps, data);
            changed = true;
        }

        if (pi != this.pageIndex) {
            this.changePageTo(pi);
            changed = true;
        }

        //if (groupByCol != this.groupByColOrdinal) {
        //    this.changeGroupByColumn(this.loadPreference('group_by_col_ordinal', -1));
        //            changed = true;
        //        }
        //        if (expandByCol != this.expandByColOrdinal) {
        //            //alert('changing expand by col to ' + expandByCol);
        //            this.changeExpandByColumn(this.loadPreference('expand_by_col_ordinal', -1));
        //            changed = true;
        //        }
        //        if (rowH != this.rowHeight) {
        //            //alert('changing rowH to ' + rowH);
        //            this.changeRowHeight(this.loadPreference('row_height', this.rowHeight));
        //            changed = true;
        //        }
        //        if (agg != this.aggregate) {
        //            //alert('changing aggregate to ' + agg);
        //            this.changeAggregate(this.loadPreference('aggregate', false));
        //            changed = true;
        //        }

        return changed;
    },

    ajaxFailed: function(req, msg, ex) {
        pivotView.done();
        alert("request failed: " + msg + "\n" + ex);
    },

    bounceData: function(fmt, data, callback) {
        var obj = this;

        $.ajax({
            type: 'POST',
            url: obj.resolveUrl(obj.url),
            data: 'action=bounce&format=' + fmt + '&data=' + data,
            dataType: 'json',
            timeout: 60000,
            success: function(json) {
                //alert('bounce error message: ' + json.errmsg);
                if (json && json.errmsg) {
                    obj.ajaxFailed(null, json.errmsg, null);
                } else {
                    if (callback) {
                        callback(json);
                        obj.done();
                    } else {
                        obj.done();
                        if (json.url.indexOf('.csv') > -1) {
                            location.href = json.url;
                        } else {
                            window.open(json.url);
                        }
                    }
                }
            },
            error: obj.ajaxFailed
        });
    },

    busy: function() {
        // show the waiting dialog
        $(".pivotViewwait").show();
    },

    done: function() {
        // hide the waiting dialog
        $(".pivotViewwait").hide();
    },

    setOptions: function(obj) {
        for (var key in obj) {
            this[key] = obj[key];
        }
    },

    postData: function(container, url, callback) {

        // default to posting to pivotviewhelper.aspx
        var action = this.url;

        if (url) {
            action = this.resolveUrl(url);
        }

        var toPost = this.urlEncodeData(container);

        //alert('url: ' + action + '\ndata: ' + toPost);

        this.busy();

        var obj = this;
        $.ajax({
            type: 'POST',
            url: action,
            data: toPost,
            dataType: 'json',
            timeout: 60000,
            success: function(json) {
                //alert('post error message: ' + json.errmsg);
                if (json && json.errmsg) {
                    obj.ajaxFailed(null, json.errmsg, null);
                } else {
                    alert(obj.debug('success: ' + json));
                    obj.initData(json, callback, allLabel);
                    obj.done();
                }
            },
            error: obj.ajaxFailed
        });
        return this;
    },

    urlEncodeData: function(obj, ignoreDotNetFields) {
        var encoded = '';
        if (!obj) {
            return;
        } else if (obj.action && obj.elements) {

            if (obj.action) {
                action = obj.action;
            }

            // assume it's a form object.
            // spin through all its children and append to data
            for (var i = 0; i < obj.elements.length; i++) {
                var el = obj.elements[i];

                // skip .net-generated fields
                if (!ignoreDotNetFields || el.name.indexOf('__') == -1) {
                    if (el.type == 'checkbox' || el.type == 'radio') {
                        // only add checkboxes / radio buttons if they're checked
                        if (el.checked) {
                            encoded += el.name + '=' + encodeURI(el.value) + '&';
                        }
                    } else {
                        // select, textarea, text, hidden, etc are always added
                        encoded += el.name + '=' + encodeURI(el.value) + '&';
                    }
                }
            }

        } else if (this.isArray(obj)) {
            for (var i = 0; i < obj.length; i++) {
                encoded += i + "=" + encodeURI(obj[i]) + "&";
            }
        } else {
            for (var key in obj) {
                encoded += key + "=" + encodeURI(obj[key]) + "&";
            }
        }

        // chop off last '&' if needed
        if (encoded.charAt(encoded.length - 1) == '&') {
            encoded = encoded.substring(0, encoded.length - 1);
        }

        encoded += '&format=json';

        return encoded;
    },

    loadData: function(data, callback) {

        var encoded = this.urlEncodeData(data, true);
        var obj = this;
        //alert('obj.selector=' + obj.selector);

        var url = this.resolveUrl(obj.url);

        //alert('url=' + url + '\nencoded=' + encoded);

        $.ajax({
            type: 'GET',
            url: url,
            data: encoded,
            dataType: 'json',
            timeout: 60000,
            success: function(json) {
                //alert('error message: ' + json.errmsg);
                if (json && json.errmsg) {
                    obj.ajaxFailed(null, json.errmsg, null);
                } else {
                    obj.initData(json, callback, allLabel);
                    obj.done();
                }
            },
            error: obj.ajaxFailed
        });
        return this;
    },

    changeExpandByColumn: function(newColOrdinal) {
        this.expandByColOrdinal = newColOrdinal;
        this.savePreference('expand_by_col_ordinal', newColOrdinal);
        this.pivotChanged = true;
        return this;
    },

    changeGroupByColumn: function(colDef, dataOrMode) {
        var pvt = this;
        // remove any auto-sorting
        if (this.groupByColOrdinal > -1) {
            this.getColumns(dataOrMode).forEach(function(col, idx) {
                if (col.sortIndex == 0 && col.autoSorted) {
                    col.sortIndex = -1;
                    col.sortDescending = false;
                    pvt.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
                    pvt.savePreference('col_sortDescending_' + col.columnName, col.sortDescending);
                    col.sortDescendingChanged = true;
                } else if (col.sortIndex > 0) {
                    col.sortIndex--;
                    pvt.savePreference('col_sortIndex_' + col.columnName, col.sortIndex);
                }
                pvt.savePreference('col_autoSorted_' + col.columnName, col.autoSorted);
            });
        }



        var groupByCol = this.getColumn(colDef, dataOrMode);

        if (!groupByCol) {

            this.groupChanged = true;
            this.groupedData = null;
            this.groupByColOrdinal = -1;
            this.savePreference('group_by_col_ordinal', this.groupByColOrdinal);

        } else {
            groupByCol.autoSorted = true;
            groupByCol.sortIndex = 0;
            groupByCol.sortDescending = false;
            pvt.savePreference('col_autoSorted_' + groupByCol.columnName, groupByCol.autoSorted);
            pvt.savePreference('col_sortIndex_' + groupByCol.columnName, groupByCol.sortIndex);
            pvt.savePreference('col_sortDescending_' + groupByCol.columnName, groupByCol.sortDescending);

            this.groupChanged = this.groupByColOrdinal != groupByCol.ordinal;

            this.groupByColOrdinal = groupByCol.ordinal;
            this.savePreference('group_by_col_ordinal', this.groupByColOrdinal);

            // resort the columns array so groupBy is always at the front
            var data = this.getDataSource(dataOrMode);
            for (var i = 0; i < data.columns.length; i++) {
                var col = data.columns[i];
                if (col.ordinal == groupByCol.ordinal) {
                    // swap 0th and current index columns (note 
                    if (i > 0) {
                        // no swapping to do, col is already in first slot
                        this.swapColumns(col.columnName, data.columns[0].columnName, dataOrMode);
                    }
                    break;
                }
            }
        }
        return this;
    },

    changeAggregate: function(newAggregate) {
        this.aggregate = newAggregate;
        this.savePreference('aggregate', this.aggregate);
        return this;
    },

    arrayToObject: function(arr, prop) {
        var ret = {};
        if (!prop) {
            for (var i = 0; i < arr.length; i++) {
                ret[arr[i]] = i;
            }
        } else {
            for (var i = 0; i < arr.length; i++) {
                ret[arr[i][prop]] = i;
            }
        }
        return ret;
    },

    arrayIndexOf: function(arr, prop, val, callback) {
        if (!this.isArray(arr)) {
            alert('following object passed to arraySortBy is not an array:\n' + arr);
            return;
        }

        if (!prop) {
            if (callback) {
                for (var i = 0; i < arr.length; i++) {
                    var munged = callback(arr[i]);
                    if (munged == val) {
                        return i;
                    }
                }
                return -1;
            } else {
                return arr.indexOf(val);
            }
        } else {
            for (var i = 0; i < arr.length; i++) {
                var test = arr[i][prop];
                if (callback) {
                    test = callback(test);
                }
                if (test == val) {
                    return i;
                }
            }
            return -1;
        }
    },

    arraySortBy: function(arr, prop, descending, makeCopy, dataType, propOffset) {

        if (!this.isArray(arr)) {
            alert('following object passed to arraySortBy is not an array:\n' + arr);
            return;
        }

        var ret = arr;
        if (makeCopy) {
            ret = arr.slice();
        }

        var lt = descending ? 1 : -1;
        var gt = descending ? -1 : 1;

        if (!prop) {
            ret.sort(function(a, b) {
                if (dataType == 'string' || isNaN(a)) {
                    if (a < b) {
                        return lt;
                    } else if (a > b) {
                        return gt;
                    } else {
                        return 0;
                    }
                } else {
                    if (descending) {
                        return b - a;
                    } else {
                        return a - b;
                    }
                }
            });
        } else {
            if (propOffset > -1) {
                ret.sort(function(a, b) {
                    if (dataType == 'string' || isNaN(a[prop][propOffset])) {
                        if (a[prop][propOffset] < b[prop][propOffset]) {
                            return lt;
                        } else if (a[prop][propOffset] > b[prop][propOffset]) {
                            return gt;
                        } else {
                            return 0;
                        }
                    } else {
                        if (descending) {
                            return b[prop][propOffset] - a[prop][propOffset];
                        } else {
                            return a[prop][propOffset] - b[prop][propOffset];
                        }
                    }
                });
            } else {
                ret.sort(function(a, b) {
                    if (dataType == 'string' || isNaN(a[prop])) {
                        if (a[prop] < b[prop]) {
                            return lt;
                        } else if (a[prop] > b[prop]) {
                            return gt;
                        } else {
                            return 0;
                        }
                    } else {
                        if (descending) {
                            return b[prop] - a[prop];
                        } else {
                            return a[prop] - b[prop];
                        }
                    }
                });
            }
        }
        return ret;

    },

    unique: function(a) {
        var r = [];
        var hash = {};
        var i = 0;
        var l = a.length;
        for (i = 0; i < l; i++) {
            hash[a[i]] = a[i];
        }
        for (i in hash) {
            if (hash.hasOwnProperty(i)) {
                r.push(hash[i]);
            }
        }
        return r;

        //        o: for (var i = 0, n = a.length; i < n; i++) {
        //            for (var x = 0, y = r.length; x < y; x++) {
        //                if (r[x] === a[i]) {
        //                    continue o;
        //                }
        //            }
        //            if (a[i] + '' != '') {
        //                r[r.length] = a[i];
        //            }
        //        }
        //        return r;
    },

    createNewColumn: function(text, i, type) {
        var col = {
            isHidden: false,
            ordinal: i,
            columnName: text,
            caption: text,
            dataType: type
        };
        return col;
    },

    getUniqueValuesForExpandByColumn: function(data) {

        var ret = [];

        if (this.expandByColOrdinal > -1) {
            ret.push(this.clone(this.getColumn(this.groupByColOrdinal)));

            var names = [];
            for (var i = 0; i < data.rows.length; i++) {
                names.push(data.rows[i][this.expandByColOrdinal]);
            }
            var obj = this;
            names = this.unique(names);
            names.forEach(function(name, i) {
                ret.push(pivotView.createNewColumn(name, i, obj.getColumn(obj.expandByColOrdinal).dataType));
            });
            ret = this.arraySortBy(ret, 'caption');
        } else {
            // copy the columns array, don't just set a reference to it
            ret = data.columns.slice();
        }
        return ret;

    },


    getPivotedValues: function(data, groupByValues, newCols) {

        var ret = {};
        for (var prop in data) {
            if (prop != 'rows' && prop != 'columns') {
                ret[prop] = data[prop];
            }
        }
        ret.isPivoted = true;
        ret.columns = newCols;
        ret.rows = [];
        ret.hiddenRowsCount = data.hiddenRowsCount;
        ret.visibleRowsCount = data.visibleRowsCount;


        var groupByCol = this.getColumn(this.groupByColOrdinal);
        var defaultAggregate = 'csv';
        switch (groupByCol.dataType) {
            case 'date':
                defaultAggregate = 'max';
                break;
            case 'number':
                defaultAggregate = 'sum';
                break;
            case 'string':
                defaultAggregate = 'csv';
                break;
        }

        var agFn = this.aggregate;
        if (!agFn) {
            agFn = defaultAggregate;
        }

        // spin through the source rows, see if its column value matches any of the group by
        for (var i = 0; i < data.rows.length; i++) {
            var row = data.rows[i];

            var offset = groupByValues.indexOf(row.values[this.groupByColOrdinal]);

            if (offset > -1) {
                // ok, we know which row the final value belongs in
                var newRow = ret.rows[offset];
                if (!newRow) {
                    // nothing exists yet for this row.
                    newRow = new Array();
                    newRow.hideMask = row.hideMask;
                    newRow.isHighlighted = row.isHighlighted;
                    newRow.isChecked = row.isChecked;

                    newRow.length = newCols.length;

                    for (var j = 0; j < newCols.length; j++) {
                        var val = row.values[newCols[j].ordinal];
                        //newRow.values[newCols[j].ordinal] = val;

                        newRow.values[newCols[j].ordinal] = new Array();

                        newRow.values[newCols[j].ordinal].push(val);
                    }


                } else {
                    // existing row.
                    // we need to apply the proper aggregate method to each value.
                    for (var j = 0; j < newCols.length; j++) {
                        if (newCols[j].ordinal == this.groupByColOrdinal) {
                            // do nothing, this is our grouping column.  its value has already been set.
                        } else {
                            var val = row.values[newCols[j].ordinal];
                            newRow.values[newCols[j].ordinal].push(val);
                        }
                    }

                }


                if (offset > ret.length) {
                    ret.rows.length = offset;
                }

                ret.rows[offset] = newRow;
            }
        }

        // now, for each new row/column, uniquify the values and concatenate them
        // TODO: change what is done based on agFn
        var obj = this;
        ret.rows.forEach(function(row, idx) {

            ret.columns.forEach(function(col, idx) {

                // sort column as needed

                if (agFn == 'csv') {
                    row.values[col.ordinal] = obj.arraySortBy(pivotView.unique(row.values[col.ordinal])).join(', ');
                } else if (agFn == 'line') {
                    row.values[col.ordinal] = pivotView.unique(row.values[col.ordinal]).join('<br />');
                } else if (agFn == 'sum') {
                    alert(row.values[col.ordinal].toSource());
                    row.values[col.ordinal] = row.values[col.ordinal].reduce(function(val, i, result) {
                        return result + val;
                    }, 0);
                } else if (agFn == 'max') {
                    row.values[col.ordinal] = row.values[col.ordinal].reduce(function(val, i, result) {
                        if (val > result) {
                            return val;
                        } else {
                            return result;
                        }
                    }, -2000000000);
                } else if (agFn == 'min') {
                    row.values[col.ordinal] = row.values[col.ordinal].reduce(function(val, i, result) {
                        if (val < result) {
                            return val;
                        } else {
                            return result;
                        }
                    }, 2000000000);
                } else if (agFn == 'avg') {
                    var sum = row.values[col.ordinal].reduce(function(val, i, result) {
                        return result + val;
                    }, 0);
                    row.values[col.ordinal] = sum / row.values[col.ordinal].length;
                } else {
                    alert('no function defined for aggregate = ' + agFn);
                }

            });
        });

        return ret;

    },

    isDef: function(o) {
        return typeof (o) != 'undefined' && o != null && o != undefined;
    },

    isArray: function(a) {
        return a && !(a.propertyIsEnumerable('length')) && typeof a === 'object' && typeof a.length === 'number';
    },

    isString: function(o) {
        return typeof o == 'string';
    },

    isNumber: function(o) {
        return typeof (o) == 'number';
    },

    isBoolean: function(o) {
        return typeof o == 'boolean';
    },
    isDate: function(o) {
        return typeof o == 'date';
    },
    isFunction: function(o) {
        return typeof o == 'function';
    },

    isObject: function(o) {
        return typeof o == 'object' && !this.isArray(o);
    },

    clone: function(obj, exceptProp) {

        if (obj == null) {
            return null;
        }

        if (this.isString(obj)) {

            // force a new copy of the string
            return obj + '';

        } else if (this.isNumber(obj)) {

            // just return the number 
            return obj;

        } else if (this.isBoolean(obj)) {

            // just return the boolean value
            return obj;
        } else if (this.isFunction(obj)) {
            // just return the function
            return obj;

        } else {

            var tgt = new Object();

            if (this.isArray(obj)) {
                tgt = new Array();
                for (var i = 0; i < obj.length; i++) {
                    tgt[i] = this.clone(obj[i], exceptProp);
                }
            }

            for (var prop in obj) {
                if (prop != exceptProp) {
                    tgt[prop] = this.clone(obj[prop], exceptProp);
                }
            }

            return tgt;
        }
    },

    noop: function(evt, bubble) {
        evt = evt || window.event;
        if (evt) {
            if (!bubble) {
                if (evt.preventDefault) {
                    evt.preventDefault();
                }
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                }
                evt.cancelBubble = true;
            }
        }

        return false;
    },

    debug: function(obj, objName) {
        var json = this.toJson(obj, objName);
        alert(json);
        return json;
    },

    toJson: function(obj, name, depth) {

        if (isNaN(depth)) {
            depth = 0;
        }
        if (name) {
            name = name + ': ';
        } else {
            name = '';
        }

        var s = '';
        if (this.isString(obj)) {
            // string
            s += name + "'" + obj.replace("'", "\\'") + "', ";
        } else if (this.isNumber(obj)) {
            // number
            s += name + obj.toString() + ', ';
        } else if (this.isBoolean(obj)) {
            // boolean
            s += name + obj.toString() + ', ';
        } else if (this.isDate(obj)) {
            s += name + "'" + obj.toString() + "', '";
        } else if (this.isArray(obj)) {
            // array
            s += name + '[';
            //s += name + '[ length: ' + obj.length + ', ';
            for (var i = 0; i < obj.length; i++) {
                s += this.toJson(obj[i], null, depth + 1);
            }
            if (obj.length > 0) {
                s = s.substring(0, s.length - 2);
            }
            s += '], ';
        } else if (this.isObject(obj)) {
            // object
            s += name + '{';
            var found = false;
            for (var key in obj) {
                s += this.toJson(obj[key], key, depth + 1);
                found = true;
            }
            if (found) {
                s = s.substring(0, s.length - 2);
            }
            s += '}, ';
        } else {
            // null or undefined, emit nothing
        }

        if (s.length > 2 && depth == 0) {
            s = s.substring(0, s.length - 2);
        }
        //		var ending = s.substring(s.length-3, s.length);
        //		if (ending == '], ' || ending == '}, '){
        //			s += '\n';
        //		}

        return s;

    },

    pivotData: function(data) {

        if (!this.allowPivoting || !this.pivotChanged) {
            //alert('no need to pivot');
            this.pivotChanged = false;
            return data;
        }

        if (isNaN(this.expandByColOrdinal)) {
            //alert('no need to pivot2');
            this.pivotChanged = false;
            return data;
        }

        //alert('pivoting...');

        this.pivotedData = null;

        this.getColumn(this.groupByColOrdinal).isHidden = false;

        // get new column names (will just be a copy of columns array from rawData if this.expandByColOrdinal is < 0)
        var newCols = this.getUniqueValuesForExpandByColumn(this.rawData);

        // fill other column values
        var pivoted = this.getPivotedValues(this.rawData, rows, newCols);

        this.pivotedData = pivoted;

        return this.pivotedData;

    },


    applyFilter: function(val, filter) {
        // TODO: add ability to do simple comparisons ('VALUE < 20', 'not Zea', 'mays or cucurbita', 'VALUE < "2009-08-01"', etc)

        //		var comp = filter.replace('*', '.*');

        if (val.match) {
            // strip out html first...
            val = this.stripHtml(val);
            var re = new RegExp(filter, "gi");
            if (val.match(re)) {
                return true;
            } else {
                return false;
            }
        } else {
            return parseFloat(val.toString()) == parseFloat(filter);
        }

    },

    filterData: function(d) {

        if (!this.allowFiltering || !this.filterChanged) {
            // filter hasn't changed, no need to spin through the rows
            //alert('no need to filter.');
            return d;
        }

        var pvt = this;

        // initialize all columns if needed
        d.columns.forEach(function(col, idx) {
            if (isNaN(col.filterMask)) {
                col.filterMask = 0;
                pvt.savePreference('col_filterMask_' + col.columnName, col.filterMask);
            }
        });


        d.hiddenRowsCount = 0;
        d.filteredRowsCount = 0;


        d.rows.forEach(function(row, idx) {

            if (isNaN(row.hideMask)) {
                row.hideMask = 0;
            }

            d.columns.some(function(col, idx) {
                // we need to possibly filter out this row.
                // we do this by masking the hideMask with the proper value.
                // here's the mask:
                //  1  =  user hidden
                //  2  =  filter for column at ordinal 0
                //  4  =  filter for column at ordinal 1
                //  8  =  ....
                // 16  =  ....
                //        (and so on up to 2**32, or 0xFFFFFFFF.  Means we can filter up to 30 columns simultaneously (LSB is taken by user hidden flag, MSB is taken by sign in int )


                // first, if the column's filtering is disabled, yank off the masked bit from the row
                if (!col.isFiltered) {

                    row.hideMask &= ~col.filterMask;

                } else {

                    var val = row.values[col.ordinal];
                    if (!pivotView.applyFilter(val, col.filterText)) {

                        // the current row fails the current filter.
                        // set its hideMask, return false to quit checking other filters.
                        row.hideMask |= col.filterMask;

                        //alert('row value "' + val + "' failed filter '" + col.filterText + "' in col " + col.caption + "\ncol mask=" + col.filterMask + "\nnew mask=" + row.hideMask);

                        return true;

                    } else {
                        row.hideMask &= ~col.filterMask;
                    }
                }

                return false;

            });

            if (row.hideMask > 0) {
                d.hiddenRowsCount++;
                if (row.hideMask > 1) {
                    d.filteredRowsCount++;
                }
            }
        });

        // mark all columns as filters haven't changed
        d.columns.forEach(function(col, i) {
            col.filterChanged = false;
        });

        // mark entire data as no filters changed
        this.filterChanged = false;

        return d;

    },

    sortData: function(d) {

        if (!this.allowSorting || !this.sortChanged) {
            // optimization -- don't rerun sorting if we never made a change in the first place
            //alert('no need to sort');
            return d;
        }


        var cols = this.arraySortBy(this.getColumns(), 'sortIndex', false, true, 'number');

        var sortCols = [];

        for (var i = 0; i < cols.length; i++) {
            if (cols[i].sortIndex > -1) {
                sortCols.push(cols[i]);
            }
        }

        var dest = d;

        if (sortCols.length == 0 && d.alternateKeyName) {
            // no sort specified, sort by primary key name ascending (set index to -2 so if they add one to sort by this one is tossed out)
            this.arraySortBy(d.rows, "fakeID", false, false, 'number');

        } else if (sortCols.length > 0) {

            // easy case, first level sort
            this.arraySortBy(d.rows, "values", sortCols[0].sortDescending, false, sortCols[0].dataType, sortCols[0].ordinal);

            // now, we need to sort within each sublevel 
            if (d.rows.length > 0 && sortCols.length > 1) {

                for (var i = 1; i < sortCols.length; i++) {

                    var prevCol = sortCols[i - 1];
                    var col = sortCols[i];

                    //alert('sorting by ' + col.columnName + ' within ' + prevCol.columnName);

                    var start = 0;

                    for (var j = 1; j < d.rows.length; j++) {

                        var prevValue = d.rows[j - 1].values[prevCol.ordinal];
                        //alert('initial value from prevous grouping: ' + prevValue);
                        var end = d.rows.length + 1;

                        // note we're incrementing the same counter as the for loop above.
                        // basically we need to process the entire array, but in chunks.
                        // the while loop below skips over values that are known to be the same
                        // for the previous sort by column.
                        while (j < d.rows.length) {
                            // determine the first row that has a different value (i.e. not in the previous grouping)
                            var nextValue = d.rows[j].values[prevCol.ordinal];
                            if (prevValue != nextValue) {
                                //alert('non matching value=' + nextValue + ' at row ' + j);
                                end = j - 1;
                                break;
                            }
                            j++;
                        }
                        if (end == d.rows.length + 1) {
                            // never found the end, assume it's d.rows.length.
                            end = d.rows.length;
                        }
                        //alert('start:' + start + '\nend:' + end);

                        if (start + 1 < end) {
                            // if there's more than 1 row in this grouping, we need to sort it

                            var subarray = d.rows.slice(start, end + 1);
                            this.arraySortBy(subarray, 'values', col.sortDescending, false, col.dataType, col.ordinal);
                            //alert(subarray.length + ' rows in grouping by column ' + col.columnName);

                            // plug the sorted sub array back into the original array
                            for (var k = 0; k < subarray.length; k++) {
                                d.rows[start + k] = subarray[k];
                            }
                        } else {
                            //alert('only 1 row in grouping by column ' + prevCol.columnName + ', its value is ' + d.rows[start][prevCol.ordinal]);
                        }
                        start = j;

                    }

                    prevCol = col;
                }
            }
        }

        this.sortChanged = false;

        return dest;

    },

    stripHtml: function(html) {
        return html.toString().replace(/<[^>]+>/gi, '');
    },

    generateGroupedRows: function(data) {
        if (this.groupByColOrdinal > -1) {
            var fakeIDsByGroup = [];
            var groupedRows = [];
            for (var i = 0; i < data.rows.length; i++) {
                var row = data.rows[i];

                // TODO: strip out html formatting, pull just the text
                var groupByValue = this.stripHtml(row.values[this.groupByColOrdinal]);

                var index = this.arrayIndexOf(groupedRows, 'groupedValue', groupByValue, this.stripHtml);
                if (index == -1) {
                    // this groupByValue has not been encountered yet.
                    // create a new row, push it to the groupedRows array
                    // push the row values into the columns
                    var newRow = this.clone(row, 'values');
                    newRow.groupedValue = groupByValue;
                    newRow.values = [];
                    newRow.originalValues = [];
                    newRow.fakeIDs = [];
                    newRow.values.length = data.columns.length;
                    for (var j = 0; j < data.columns.length; j++) {
                        var col = data.columns[j];
                        var val = row.values[col.ordinal];
                        newRow.originalValues[col.ordinal] = [val];
                    }
                    newRow.fakeIDs.push(row.fakeID);
                    groupedRows.push(newRow);
                } else {
                    newRow.fakeIDs.push(row.fakeID);
                    for (var j = 0; j < data.columns.length; j++) {
                        col = data.columns[j];
                        if (col.ordinal == this.groupByColOrdinal) {
                            // do nothing, this is our grouping column.  its value has already been set.
                        } else {
                            val = row.values[col.ordinal];
                            newRow.originalValues[col.ordinal].push(val);
                        }
                    }
                }
            }

            return groupedRows;

        } else {
            alert('should never get here -- getUniqueValuesForGroupByColumn but groupBy is < 0');
        }
    },


    groupData: function(data) {

        if (isNaN(this.groupByColOrdinal)) {
            //alert('changing to default group by column of ' + data.alternateKeyName);
            this.changeGroupByColumn(data.alternateKeyName);
        }

        if (!this.allowGrouping) {
            return data;
        }


        if (!this.groupChanged && this.groupedData != null) {
            // optimization -- don't rerun grouping if we never made a change in the first place
            this.groupedData.rows = this.formatGroupedRows(this.groupedData.rows, this.groupedData.columns, this.getColumn(this.groupByColOrdinal, this.groupedData), false);
            // plug rows into grouped data
            return this.groupedData;
        }

        if (this.groupByColOrdinal < 0) {
            // no grouping applied. skip all grouping logic.
            this.groupedData = null;
            this.groupChanged = false;
            return data;
        }

        // we get here, grouping must be applied.
        // first time through, just clone the source data (wasteful, fix this later!)
        data = this.clone(data, 'rows');

        // change data mode to grouped (i.e. getDataSource() should return this.groupedData by default now instead of this.rawData
        //this.dataOrMode = 'grouped';

        // grouping implies sorting on the grouped column first.
        var groupingCol = this.getColumn(this.groupByColOrdinal, data);
        if (!groupingCol) {
            alert("bad grouping column ordinal=" + this.groupByColOrdinal);
        } else {
            this.prioritizeSortColumn(groupingCol, 'raw');
        }

        // generate a new array of rows that are grouped as needed
        var groupedRows = this.generateGroupedRows(this.rawData);

        var formattedGroupedRows = this.formatGroupedRows(groupedRows, data.columns, groupingCol, true);
        // plug rows into grouped data
        data.rows = groupedRows;
        this.groupedData = data;
        this.groupChanged = false;

        return data;

    },

    formatGroupedRows: function(groupedRows, cols, groupingCol, init) {

        var defaultAggregate = 'csv';
        switch (groupingCol.dataType) {
            case 'date':
                defaultAggregate = 'max';
                break;
            case 'number':
                defaultAggregate = 'sum';
                break;
            case 'string':
                defaultAggregate = 'csv';
                break;
        }

        // now, for each new row/column, uniquify the values and concatenate them
        // TODO: change what is done based on agFn
        var agFn = 'line';
        var obj = this;


        // determine which column(s) need to be recalculated
        // this is an expensive process, so we narrow things down as much as possible outside the groupedRows loop.
        var colsToRefresh = [];
        //		for(var idx = 0;idx<cols.length;idx++){
        //			var col = cols[idx];
        cols.forEach(function(col, idx) {
            if (col.filterChanged || col.sortDescendingChanged || init) {
                colsToRefresh.push(col);
            }
        });
        //		}


        if (colsToRefresh.length > 0) {
            groupedRows.forEach(function(row, idx) {
                colsToRefresh.forEach(function(col, idx2) {

                    //					if (this.filterChanged || col.sortDescendingChanged || !row.values[col.ordinal]){

                    // TODO: optimize to only regen data when source has changed (such as sort direction changed, filter applied, etc)

                    if (agFn == 'csv') {
                        row.values[col.ordinal] = obj.arraySortBy(obj.unique(row.originalValues[col.ordinal]), null, col.sortDescending).join(', ');
                    } else if (agFn == 'line') {
                        row.values[col.ordinal] = obj.arraySortBy(obj.unique(row.originalValues[col.ordinal]), null, col.sortDescending).join('<br />');
                    } else if (agFn == 'sum') {
                        row.values[col.ordinal] = row.originalValues[col.ordinal].reduce(function(val, i, result) {
                            return result + val;
                        }, 0);
                    } else if (agFn == 'count') {
                        row.values[col.ordinal] = obj.unique(row.originalValues[col.ordinal]).length;
                    } else if (agFn == 'max') {
                        row.values[col.ordinal] = row.originalValues[col.ordinal].reduce(function(val, i, result) {
                            if (val > result) {
                                return val;
                            } else {
                                return result;
                            }
                        }, -2000000000);
                    } else if (agFn == 'min') {
                        row.values[col.ordinal] = row.originalValues[col.ordinal].reduce(function(val, i, result) {
                            if (val < result) {
                                return val;
                            } else {
                                return result;
                            }
                        }, 2000000000);
                    } else if (agFn == 'avg') {
                        var sum = row.originalValues[col.ordinal].reduce(function(val, i, result) {
                            return result + val;
                        }, 0);
                        row.values[col.ordinal] = sum / row.originalValues[col.ordinal].length;
                    } else {
                        alert('no function defined for aggregate = ' + agFn);
                    }

                });
            });
        }


        return groupedRows;

    },


    rebindEvents: function() {
        $(".sortSource").sortable({
            //			containment: $(this).parent(),
            connectWith: '.sortTarget',
            cursorAt: 'left : 5',
            distance: 5,
            delay: 200,
            revert: 'invalid',
            snap: true,
            snapMode: 'inner',
            snapTolerance: 20,
            zIndex: 5
            //			update: function(event, ui) { pivotView.applySortChanges(); }
        });

        $(".sortTarget").sortable({
            containment: 'parent',
            //			connectWith: '.sortSource',
            cursorAt: 'left : 5',
            distance: 5,
            delay: 200,
            //			revert: 'invalid',
            snap: true,
            snapMode: 'inner',
            snapTolerance: 20,
            zIndex: 5,
            update: function(event, ui) { pivotView.applySortChanges().refresh().noop(event); }
        });


        $(".droppable").droppable({
            drop: function(event, ui) {
                // flip-flop the two elements
                //alert(ui.draggable.html() + '\n' + $(this).html());
                $(this).droppable('destroy');
                pivotView.shiftColumns(ui.draggable.text(), $("span", $(this)).text()).refresh();
            },
            activeClass: 'ui-droppable-active',
            hoverClass: 'ui-droppable-hover',
            //			deactivate: function(event, ui) { alert('deactivated'); },
            //			over: function(event, ui) { alert('over'); },
            tolerance: 'intersect'
        });
        $(".draggable").draggable({
            axis: 'x',
            cursorAt: { left: 3 },
            distance: 30,
            opacity: 0.9,
            revert: 'invalid',
            snap: true,
            snapMode: 'inner',
            snapTolerance: 20,
            zIndex: 5,
            start: function(event, ui) { $(this).addClass("ui-draggable-active"); },
            stop: function(event, ui) { $(this).removeClass("ui-draggable-active"); }
        });

        $("textarea.exporterTextArea").focus(function() {
            this.select();
        });

        $("select[name='exportRows']").val(this.exportRows);
        $("select[name='exportFormat']").val(this.exportFormat);

        //*

        if (this.allowFilteringAutoComplete) {
            $(".ac_results").hide();
            this.getDataSource().columns.forEach(function(col, i) {
                $(".filterTextBox.col" + i).autocompleteArray(
					col.values,
					{
					    delay: 10,
					    minChars: 1,
					    matchSubset: 1,
					    onItemSelect: pivotView.selectValue,
					    onFindValue: pivotView.selectValue,
					    autoFill: true,
					    maxItemsToShow: 30,
					    srcClass: ".filterTextBox.col" + i
					});
            });
        }
        //*/

        this.refreshExporterTextArea();

        $(".pivotViewwait").hide();
    },

    selectValue: function(li) {
        if (!li) {
            return;
        } else {
            alert($(li).parent().html());
            alert(li.selectValue);
        }
    },

    htmlForTableBody: function(data) {

        var ret = "";


        // ok, we can't just jump to the pageSize * pageIndex
        // this is because when we hide rows (either the user selected to hide them or applied a filter)
        // we don't remove them from the array, we just toggle a property.
        // so jumping to a specific page won't end us up in the right position.
        // we must spin through the array until we've passed pageIndex * pageSize rows.


        var start = this.pageIndex * this.pageSize;
        var end = (this.pageIndex + 1) * this.pageSize;
        // since we want to process all rows, do not include the data.hiddenRowsCount in our calc
        if (end > data.rows.length) {
            end = data.rows.length;
        }
        var done = data.rows.length == 0;
        var i = 0;
        var skipCount = 0;
        var outputCount = 0;

        if (data.rows.length > 0) {

            while (!done) {
                var row = data.rows[i];
                if (!row) {
                    alert('undefined row at index=' + i);
                }
                if (!row.hideMask) {
                    skipCount++;
                    if (skipCount >= start) {
                        outputCount++;
                        var rowhtml = '';
                        if (this.showCheckBoxes) {
                            var checked = row.isChecked ? "checked='checked'" : "";
                            var pkValue = data.primaryKeyName ? row.values[this.getOrdinal(data.primaryKeyName, data)] : '';
                            rowhtml += "<td><input type='checkbox' class='pivotViewCheckBox' " + checked + " name='pivotViewCheckBox" + i + "' onclick='javascript:pivotView.toggleCheckBox(this.checked, " + i + ");' value='" + pkValue + "' /></td>";
                        }
                        for (var j = 0; j < data.columns.length; j++) {
                            var col = data.columns[j];
                            if (!col.isHidden) {
                                rowhtml += "<td><div style='padding-right:20px' class='limitbox" + this.rowHeight + "'>" + row.values[col.ordinal] + "</div></td>";
                                //								rowhtml += "<td>" + row.values[col.ordinal] + "</td>";

                                //							if (col.ordinal == this.groupByColOrdinal && this.allowPivoting) {
                                //								rowhtml = '<td>' + row.values[col.ordinal] + '</td>' + rowhtml;
                                //							} else {
                                //								rowhtml += '<td>' + row.values[col.ordinal] + '</td>';
                                //							}

                            }
                        }

                        // prepend an empty column for the pivot column if needed
                        if (this.groupByColOrdinal < 0 && (this.allowGrouping || this.allowPivoting)) {

                            rowhtml = '<td>&nbsp;</td>' + rowhtml;
                        }

                        // tack on a column for hiding the row
                        rowhtml += "<td><a class='jsanchor hover' title='Hide Row' href='javascript:Hide Row' onclick='javascript:return pivotView.toggleRow(" + i + ").refresh().noop(event);'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt='Hide Row' title='Hide Row' /></a></td>";

                        //					rowhtml = '<tr onclick="javascript:return pivotView.rowClick(this, ' + i + ', null, event); return true;" ' + (row.isHighlighted ? 'class="hilite"' : '') + ' >' + rowhtml + '</tr>';
                        rowhtml = '<tr onclick="javascript:return pivotView.rowClick(this, ' + i + ', null, event); return true;" ' + (row.isHighlighted ? 'class="hilite"' : '') + ' >' + rowhtml + '</tr>';
                        //					rowhtml = '<tr >' + rowhtml + '</tr>';
                        ret += rowhtml;
                    }
                }
                done = outputCount == this.pageSize || i == data.rows.length - 1;
                i++;
            }
        }

        return ret;
    },

    markSelected: function(val1, val2, defaultIfEmpty) {
        if (val1 == val2 || (!val1 || !val2 && defaultIfEmpty)) {
            return " selected='selected'";
        } else {
            return "";
        }
    },

    htmlForAggregateDropdown: function(ordinal) {

        // create the function to apply to data
        var ret = '';
        if (isNaN(ordinal)) {
            ret += "<select name='aggregate' title='Aggregate Function' onchange='javascript:pivotView.changeAggregate(this.options[this.selectedIndex].value).refresh();'>";
        } else {
            ret += "<select name='aggregate" + ordinal + "' title='Aggregate Function' onchange='javascript:pivotView.changeColumnAggregate(this.options[this.selectedIndex].value, " + ordinal + ").refresh();'>";
        }

        // numeric
        var col = this.getColumn(this.expandByColOrdinal);
        if (!col) {
            ret += "<option value=''>(n/a)</option>";
        } else {
            switch (col.dataType) {
                case 'number':
                    ret += "<option value='sum' " + this.markSelected('sum', this.aggregate, true) + " >Sum</option>";
                    ret += "<option value='avg'" + this.markSelected('avg', this.aggregate) + ">Average</option>";
                    ret += "<option value='max'" + this.markSelected('max', this.aggregate) + ">Maximum</option>";
                    ret += "<option value='min'" + this.markSelected('min', this.aggregate) + ">Minimum</option>";
                    break;
                case 'date':
                    ret += "<option value='max'" + this.markSelected('max', this.aggregate, true) + ">Maximum</option>";
                    ret += "<option value='min'" + this.markSelected('min', this.aggregate) + ">Minimum</option>";
                    break;
                case 'string':
                    ret += "<option value='csv'" + this.markSelected('csv', this.aggregate, true) + ">Comma Separated</option>";
                    ret += "<option value='line'" + this.markSelected('line', this.aggregate) + ">Line Separated</option>";
                    break;
            }

        }

        ret += "</select>";

        return ret;
    },

    htmlForExpandByColDropdown: function(data) {

        // create the dropdown for expandByCol
        var ret = "Expand By: <select title='Expand Column' onchange='javascript:pivotView.changeExpandByColumn(this.options[this.selectedIndex].value).refresh();'><option value='-1'>(None)</option>";
        var sel = '';

        for (var i = 0; i < data.columns.length; i++) {
            var col = data.columns[i];
            sel = (col.ordinal == this.expandByColOrdinal ? ' selected="selected" ' : '');
            if (col.ordinal != this.groupByColOrdinal && !col.isHidden) {
                ret += "<option value='" + col.ordinal + "' " + sel + ">" + col.caption + "</option>";
            }
        }
        ret += "</select>";
        return ret;
    },

    htmlForGroupByColDropdown: function(data) {

        //return this.getColumn(this.groupByColOrdinal).caption;


        // create the dropdown picker for groupByCol
        var ret = this.loadLabel('lblGroupBy', 'Group By') + ":<br /><select title='Group By Column' onchange='javascript:pivotView.changeGroupByColumn(this.options[this.selectedIndex].value).refresh();'><option value='-1'>(None)</option>";
        for (var i = 0; i < data.columns.length; i++) {
            var col = data.columns[i];
            if (!col.isHidden) {
                sel = (col.ordinal == this.groupByColOrdinal ? ' selected="selected" ' : '');
                ret += "<option value='" + col.ordinal + "' " + sel + ">" + col.caption + "</option>";
            }
        }
        ret += "</select>";

        return ret;

    },

    htmlForPager: function(data, selectedPageSize) {
        var pager = "<span class='pager' style='margin-left:30px'><nobr>";
        pager += this.loadLabel('lblShow', 'Show') + "&nbsp;<select title='Change Page Size' onchange='javascript:pivotView.changePageSize(this.options[this.selectedIndex].value).refresh();'>";

        var pageSizes = [10, 25, 50, 100, 250, 500, 1000, 2500, 5000];
        var pageSizeOptions = "";
        for (var j = 0; j < pageSizes.length; j++) {
            if (pageSizes[j] >= (data.rows.length - data.hiddenRowsCount)) {
                pageSizeOptions += "<option value='" + (data.rows.length - data.hiddenRowsCount) + "'>" + this.loadLabel('lblAll', 'All') + "</option>";
                break;
            } else {
                pageSizeOptions += "<option value='" + pageSizes[j] + "'>" + pageSizes[j] + "</option>";
            }
        }
        pageSizeOptions = pageSizeOptions.replace("value='" + selectedPageSize + "'>", "value='" + selectedPageSize + "' selected='selected'>");

        pager += pageSizeOptions;

        pager += "</select>&nbsp;" + this.loadLabel('lblItems', 'items') + "&nbsp;&nbsp;&nbsp;";
        if (this.pageIndex == 0) {
            pager += "&lt;&lt;&nbsp;&lt;";
        } else {
            pager += "<a class='jsanchor' href='javascript:First Page of Data' title='Jump to First Page' onclick='javascript:return pivotView.changePageTo(0).refresh().noop(event);'>&lt;&lt;</a>";
            pager += "&nbsp;<a class='jsanchor' href='javascript:Previous Page of Data' title='Go to Previous Page' onclick='javascript:return pivotView.changePageTo(-1).refresh().noop(event);'>&lt;</a>";
        }

        pager += "&nbsp;<select title='Change Current Page' onchange='javascript:pivotView.changePageTo(this.options[this.selectedIndex].value).refresh();'>";
        if (this.pageCount == 0) {
            pager += "<option value='0'>0</option>";
        } else {
            for (var i = 0; i < this.pageCount; i++) {
                var lastItem = (this.pageSize * (i + 1));
                var done = false;
                if (lastItem > (data.rows.length - data.hiddenRowsCount)) {
                    lastItem = (data.rows.length - data.hiddenRowsCount);
                    done = true;
                }
                var text = ((this.pageSize * i) + 1) + " - " + lastItem;
                var sel = (i == this.pageIndex ? "selected='selected'" : "");
                pager += "<option value='" + i + "'" + sel + ">" + text + "</option>";
                if (done) {
                    break;
                }
            }
        }

        var hidFiltText = '';
        if (data.hiddenRowsCount && data.filteredRowsCount) {
            hidFiltText += " (" + (data.hiddenRowsCount - data.filteredRowsCount) + " hidden, " + data.filteredRowsCount + " filtered)";
        } else if (data.hiddenRowsCount) {
            hidFiltText += " (" + data.hiddenRowsCount + " hidden)";
        } else if (data.filteredRowsCount) {
            hidFiltText += " (" + data.filteredRowsCount + " filtered)";
        }

        pager += "</select> " + this.loadLabel('lblOf', 'of') + " <span title='" + hidFiltText + "'>" + (data.rows.length - data.hiddenRowsCount) + "</span>";

        if (this.pageIndex == this.pageCount - 1) {
            pager += "&nbsp;&gt;&nbsp;&gt;&gt;";
        } else {
            pager += "&nbsp;<a class='jsanchor' href='javascript:Next Page of Data' title='Go to Next Page' onclick='javascript:return pivotView.changePageTo(-2).refresh().noop(event);'>&gt;</a>";
            pager += "&nbsp;<a class='jsanchor' href='javascript:Last Page of Data' title='Jump to Last Page' onclick='javascript:return pivotView.changePageTo(-3).refresh().noop(event);'>&gt;&gt;</a>";
        }
        pager += "</nobr></span>";

        return pager;
    },

    htmlForExportingPopup: function(data) {
        var exporter = "&nbsp;&nbsp;<a class='jsanchor popupAnchor' title='Export Data' href='javascript:Export Data...' onclick='return pivotView.toggleExporter().noop(event, true);'>" + this.loadLabel('lblExport', 'Export') + "...</a>";
        exporter += "<div class='exporter pivotPopup' onclick='javascript:return pivotView.noop(event);'>";

        //var exportRows = $("input.exporter[name='exportRows']:checked").val();

        exporter += "<select title='Export Options' name='exportRows' onchange='javascript:pivotView.refreshExporterTextArea();'><option value='all'>All Rows</option><option value='page'>Current Page</option><option value='hilite'>Highlighted Rows</option><option value='checked'>Checked Rows</option></select><br />";

        //exporter += "<label><input type='radio' name='exportRows' value='all' checked='checked' />All Rows</label><br />";
        //exporter += "<label><input type='radio' name='exportRows' value='hilite' />Highlighted Rows</label><br />";
        //exporter += "<label><input type='radio' name='exportRows' value='page' />Current Page</label><br />";

        //				var tab = '', csv = '';
        //				if ($("input.exporter[name='exportType']:checked").val() == 'tab'){
        //					tab = "checked='checked' ";
        //				} else {
        //					csv = "checked='checked' ";
        //				}
        exporter += "<select title='Export Format' name='exportFormat' onchange='javascript:pivotView.refreshExporterTextArea();'><option value='csv'>Comma Separated (csv)</option><option value='tab'>Tab Delimited (txt)</option></select><br />";
        //				exporter += "<label><input type='radio' name='exportType' value='csv' " + csv + "/>Comma Separated (csv)</label><br />";
        //				exporter += "<label><input type='radio' name='exportType' value='' " + tab + "/>Tab Delimited (csv)</label><br />";
        exporter += "<div class='exporterRowCount'></div>";
        exporter += "<p><a class='jsanchor' href='javascript:Export Data To File' onclick='javascript:return pivotView.exportDataToFile().noop(event);'><img src='" + this.resolveUrl('~/images/btn_exportdata.gif') + "' title='Export Data' alt='Export Data' /></a>";
        if (document.all) {
            // cheesy way of detecting IE
            exporter += "<br /><a class='jsanchor' href='trustedsite.aspx' onclick='javascript:location.href=\"trustedsite.aspx\";'>Why does 'Information Bar' appear?</a><br />";
        }
        exporter += '</p>';

        exporter += "<a class='jsanchor' href='javascript:Show Data to Drag to Spreadsheet' onclick='javascript:return pivotView.toggleExporterTextArea().noop(event);' >" + this.loadLabel('lblDragTo', 'Drag to spreadsheet') + "...</a><br />";
        exporter += "<textarea class='exporterTextArea' onfocus='javascript:this.select();'></textarea><br />";
        //				exporter += "<a href='javascript:Select All' onclick='$(\"textarea.exporterTextArea\").each(function(){ this.select(); }); return false;'>Select All</a>";
        //exporter += "<input type='button' value='Done' onclick='pivotView.toggleExporter().noop(event, true);' />";
        exporter += "<input type='button' value='" + this.loadLabel('lblDone', 'Done') + "' onclick='pivotView.toggleExporter().noop(event, true);' />";
        exporter += "</div>";
        return exporter;
    },

    htmlForOptionsPopup: function(data) {
        var pivotOptions = "<a href='javascript:Edit Options' title='Edit Options'  class='jsanchor popupAnchor' onclick='$(\".pivotOptions\").slideToggle(\"fast\"); return false;'>" + this.loadLabel('lblOptions', 'Options') + ":</a>";

        pivotOptions += "<div class='pivotOptions pivotPopup' style='display:" + (this.pivotOptionsVisible ? "block" : "none") + ";'>";
        pivotOptions += "<table cellspacing='0' cellpadding='0' border='0'><tr>";

        // sort columns header...
        if (this.allowSorting) {
            pivotOptions += "<th><nobr>" + this.loadLabel('lblSortColumns', 'Sorted Columns') + " <a class='jsanchor' href='javascript:Choose Columns...' onclick='javascript:$(\".columnSortPicker\").slideToggle(\"fast\"); return pivotView.noop(event);' title='Choose Columns to Sort By...'><img src='" + this.resolveUrl('~/images/add.gif') + "' /></a></nobr></th>";
        }

        // filter columns header...
        if (this.allowFiltering) {
            pivotOptions += "<th>" + this.loadLabel('lblFilterColumns', 'Filtered Columns') + "<a href='javascript:Add Filtered Column...' title='Add Filtered Column...' onclick='javascript:$(\".columnFilterPicker\").slideToggle(\"fast\"); return pivotView.noop(event);'><img src='" + this.resolveUrl('~/images/add.gif') + "' alt='add' /></a></th>";
        }

        pivotOptions += "</tr><tr>";

        var removeAllSortedColumns = '';
        var removeAllFilteredColumns = '';
        var sortedLines = '';
        var filteredLines = '';

        // sort columns portion...
        if (this.allowSorting) {

            pivotOptions += "<td><div class='columnSortPicker hide'>";
            var lines = "";
            var line = '';
            for (var i = 0; i < data.columns.length; i++) {
                var col = data.columns[i];
                if (col.sortIndex < 0) {
                    line = "<div><a title='Sort By " + col.caption + "' href='javascript:Sort By Column' class='jsanchor' onclick='javascript:return pivotView.toggleSort(" + col.ordinal + ", true).refresh().noop(event);'>" + col.caption + "<img src='" + this.resolveUrl('~/images/add.gif') + "' alt=' add' /></a></div>";
                    lines += line;
                }
            }
            pivotOptions += lines;
            pivotOptions += "</div>";

            var cols = this.arraySortBy(data.columns, 'sortIndex', false, true, 'number');
            for (i = 0; i < cols.length; i++) {
                var col = cols[i];
                if (col.sortIndex > -1) {
                    line = "<li title='Drag and drop to change sorting precedence'><nobr><div style='float:left;width:100px'>" + col.caption + "</div><div style='float:right'><a title='Change Sort Direction' href='javascript:Change Sort Direction' onclick='javascript:return pivotView.toggleSortDir(" + col.ordinal + ", true).refresh().noop(event);'><img style='border:none' src='" + this.resolveUrl('~/images/' + (col.sortDescending ? "up.gif" : "down.gif")) + "' alt='Change Sort Direction' /></a> <a class='jsanchor' title='Do not sort by this column' href='javascript:Remove Sort By Column' onclick='javascript:return pivotView.toggleSort(" + col.ordinal + ", true).refresh().noop(event);'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt=' remove' /></a></div></nobr></li>";
                    sortedLines += line;
                }
            }
            if (sortedLines != '') {
                pivotOptions += "<ul class='sortTarget' >" + sortedLines + "</ul>";
            }
            pivotOptions += "</td>";

            //				sortedColumns += "<div style='float:left'>Remove All Sorting</div><div style='float:right' ><a href='javascript:Remove All Sorting' class='jsanchor' onclick='javascript:return pivotView.removeAllSorts().refresh().noop(event);' title='Remove All Sorting'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt=' ' /></a></p>";
            removeAllSortedColumns = "<p>" + this.loadLabel('lblRemoveSort', 'Remove All Sorting') + "&nbsp;&nbsp;&nbsp;<a href='javascript:Remove All Sorting' class='jsanchor' onclick='javascript:return pivotView.removeAllSorts().refresh().noop(event);' title='Remove All Sorting'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt=' ' /></a></p>";


        }


        // filter columns portion...
        if (this.allowFiltering) {
            filteredLines = '';
            pivotOptions += "<td><div class='columnFilterPicker hide'>";
            data.columns.forEach(function(col, i) {
                if (!col.isFiltered) {
                    var line = "<div onclick='javascript:return pivotView.toggleFilter(" + col.ordinal + ", \"\", true, false).refresh().noop(event);'><a href='javascript:Filter By Column' class='jsanchor' >" + col.caption + "<img src='" + pivotView.resolveUrl('~/images/add.gif') + "' alt=' add' style='text-align:right'/></a></div>";
                    filteredLines += line;
                }
            });
            pivotOptions += filteredLines;
            pivotOptions += "</div><div>";

            filteredLines = "";
            data.columns.forEach(function(col, i) {
                if (col.isFiltered) {
                    var line = pivotView.htmlForFilterEditor(col, false, col.caption);
                    filteredLines += line;
                }
            });
            pivotOptions += filteredLines;
            pivotOptions += "</div>";

            pivotOptions += "</td>";

            removeAllFilteredColumns += "<p>Remove All Filters&nbsp;&nbsp;&nbsp;<a href='javascript:Remove All Filters' class='jsanchor' onclick='javascript:return pivotView.removeAllFilters().refresh().noop(event);' title='Remove All Filters'><img src='" + pivotView.resolveUrl('~/images/remove.gif') + "' alt=' ' /></a></p>";
        }

        pivotOptions += "</tr><tr>";

        if (this.allowSorting) {
            pivotOptions += "<td>"
            if (sortedLines == "") {
                // no sorted columns.
                pivotOptions += "<p>(" + this.loadLabel('lblNone', 'none') + ")</p>";
            } else {
                pivotOptions += removeAllSortedColumns;
            }
            pivotOptions += "</td>";

        }

        if (this.allowFiltering) {
            pivotOptions += "<td>"
            if (filteredLines == "") {
                pivotOptions += "<p>(" + this.loadLabel('lblNone', 'none') + ")</p>";
            } else {
                pivotOptions += removeAllFilteredColumns;
            }
            pivotOptions += "</td>";
        }


        // close table nicely, show Done button
        pivotOptions += "</tr></table>";

        // row size limits
        pivotOptions += "<fieldset><legend>" + this.loadLabel('lblRowHeight', 'Row Height Limit') + "</legend>";

        var checked = this.rowHeight == 50 ? "checked='checked' " : "";
        pivotOptions += "<label><input type='radio' name='pivoterRowHeight' onclick='javascript:pivotView.changeRowHeight(50).refresh();' value='50' " + checked + " />" + this.loadLabel('lblSmall', 'Small') + "</label>&nbsp;";

        checked = this.rowHeight == 100 ? "checked='checked' " : "";
        pivotOptions += "<label><input type='radio' name='pivoterRowHeight' onclick='javascript:pivotView.changeRowHeight(100).refresh();' value='100' " + checked + " />" + this.loadLabel('lblMedium', 'Medium') + "</label>&nbsp;";

        checked = this.rowHeight == 300 ? "checked='checked' " : "";
        pivotOptions += "<label><input type='radio' name='pivoterRowHeight' onclick='javascript:pivotView.changeRowHeight(300).refresh();' value='300' " + checked + " />" + this.loadLabel('lblLarge', 'Large') + "></label>&nbsp;";

        checked = this.rowHeight == -1 ? "checked='checked' " : "";
        pivotOptions += "<label><input type='radio' name='pivoterRowHeight' onclick='javascript:pivotView.changeRowHeight(-1).refresh();' value='-1' " + checked + " />" + this.loadLabel('lblUnLimit', 'Unlimited') + "</label></fieldset>";

        pivotOptions += "<input type='button' value='" + this.loadLabel('lblDone', 'Done') + "' onclick='javascript:pivotView.hidePivotOptions(); $(\".columnSortPicker\").hide();' /></div>";

        return pivotOptions;
    },

    htmlForHeaderRow: function(data) {

        var headerRow = '';
        //var columnRowAdder = '<div class="columnRowAdder pivotPopup" onclick="javascript:return pivotView.noop(event);"><fieldset><legend>Show Columns</legend>';
        var columnRowAdder = "<div class='columnRowAdder pivotPopup' onclick='javascript:return pivotView.noop(event);'><fieldset><legend>" + this.loadLabel('lblShowColumn', 'Show Columns') + "</legend>";
        var colCount = 0;
        // we always display the groupby column if pivoting is enabled

        // gather some stats on the columns
        var visibleColCount = 0;
        var hiddenColCount = 0;

        if (this.showCheckBoxes) {
            headerRow += "<th><input type='checkbox' name='pivotViewCheckBoxAll' onclick='javascript:pivotView.toggleAllCheckBoxes(this.checked);' /></th>";
        }

        for (var i = 0; i < data.columns.length; i++) {
            var col = data.columns[i];

            //alert(col.caption + '\nordinal=' + col.ordinal + '\ngroupby ordinal=' + this.groupByColOrdinal);

            if (col.isHidden) {
                hiddenColCount++;
            } else {
                visibleColCount++;
            }

            var outputColumn = true;
            if (col.ordinal == this.groupByColOrdinal || this.groupByColOrdinal < 0) {
                if (this.allowGrouping || this.allowPivoting) {
                    if (i == 0) {
                        var groupByColDropdown = this.htmlForGroupByColDropdown(this.getDataSource()); //data);
                        headerRow += '<th>' + groupByColDropdown + '</th>'; // + headerRow;
                        if (this.expandByColOrdinal > -1) {
                            outputColumn = true;
                        } else {
                            outputColumn = this.groupByColOrdinal < 0;
                        }
                        colCount++;
                    }
                }
            }

            if (outputColumn) {
                if (!col.isHidden) {

                    var sortDir = "down";
                    var dirDesc = "decending";
                    var sortImg = "";
                    if (col.sortIndex > -1) {
                        if (col.sortDescending) {
                            sortDir = "up";
                            dirDesc = "ascending";
                        }
                        sortImg = "<a href='javascript:Toggle Sort' onclick='javascript:return pivotView.toggleSortDir(\"" + col.columnName + "\", false).refresh().noop(event);' title='Sort " + dirDesc + "'><img src='" + this.resolveUrl('~/images/' + sortDir + '.gif') + "' alt=' " + dirDesc + "' /></a>";
                    }

                    var filterImg = "";
                    if (col.isFiltered) {
                        filterImg = "<a href='javascript:Edit Filter' onclick='javascript: $(\".filterEditor.col" + col.ordinal + "\").slideToggle(\"fast\", function() { $(\".filterTextBox.col" + col.ordinal + "\", $(\".filterEditor.col" + col.ordinal + "\")).focus().select(); }); return false;' title='Filtering on \"" + col.filterText + "\"'><img src='" + this.resolveUrl('~/images/filter.gif') + "' alt='edit filter' /></a>";
                    }

                    var droppable = this.allowColumnMovement ? "droppable" : "";
                    var draggable = this.allowColumnMovement ? "draggable" : "";

                    headerRow += '<th title="Drag and drop to change column position" class="' + droppable + ' col' + col.ordinal + '"><span class="' + draggable + '"><nobr>' + col.caption + ' ' + sortImg + filterImg + '<a class="popupAnchor" href="javascript:Edit Column Preferences" onclick="javascript:$(\'.columnPref.col' + col.ordinal + '\').slideToggle(\'fast\', function() { $(\'.filterTextBox.col' + col.ordinal + '\').focus().select(); }); return false;" title="Edit Preferences for ' + col.caption + ' Column"><img class="hidereserve" src="' + this.resolveUrl('~/images/prefs.gif') + '" alt=" options" /></a></nobr></span>';

                    var filterEdit = this.htmlForFilterEditor(col, true);
                    headerRow += filterEdit;

                    var columnPref = this.htmlForColumnPrefPopup(col, visibleColCount, sortDir, dirDesc);

                    headerRow += columnPref + "</th>";
                    colCount++;
                } else {
                    columnRowAdder += "<div><a class='jsanchor' href='javascript:Toggle Column' onclick='javascript:return pivotView.toggleColumn(\"" + col.columnName + "\").refresh().noop(event);' title='Show " + col.caption + " Column'>" + col.caption + " <img src='" + this.resolveUrl('~/images/add.gif') + "' alt='add' /></a></div>";
                }
            }
        }
        //		if (this.groupByColOrdinal > -1 && this.allowPivoting) {
        //			headerRow = '<th>' + groupByColDropdown + '</th>' + headerRow;
        //			colCount++;
        //		}
        //		

        // don't forget to tack on a 'show all'
        if (hiddenColCount > 1) {
            columnRowAdder += "<a class='jsanchor' href='javascript:Show All Columns' onclick='javascript:return pivotView.showAllColumns().refresh().noop(event);' title='Show All Columns'>(" + this.loadLabel('lblAll', 'All') + ")<img src='" + this.resolveUrl('~/images/add.gif') + "' alt=' show all' /></a>";
        } else if (hiddenColCount == 0) {
            columnRowAdder += "(" + this.loadLabel('lblNoColumn', 'no hidden columns') + ")";
        }
        columnRowAdder += "</fieldset>";


        // add option to add back hidden rows
        columnRowAdder += "<fieldset><legend>" + this.loadLabel('lblShowRows', 'Show Rows') + "</legend>";
        if (data.hiddenRowsCount > 0) {
            columnRowAdder += "<a class='jsanchor' href='javascript:Show All Unfiltered Rows' onclick='javascript:return pivotView.showAllNonFilteredRows().refresh().noop(event);' title='Show All Rows'>" + this.loadLabel('lblAllRows', 'All Unfiltered Rows') + " <img src='" + this.resolveUrl('~/images/add.gif') + "' alt=' show all unfiltered rows' /></a>";
        } else {
            columnRowAdder += "(" + this.loadLabel('lblNoRow', 'no hidden rows') + ")";
        }

        columnRowAdder += "</fieldset></div>";

        // tack on a column for adding back the hidden columns or rows if needed
        if (hiddenColCount > 0 || data.hiddenRowsCount > 0) {
            headerRow += '<th><a class="jsanchor popupAnchor" href="javascript:Show Hidden Columns and Rows..." onclick="javascript:$(\'.columnRowAdder\').slideToggle(\'fast\'); return false;" title="Show Hidden Columns and Rows..."><img src="' + this.resolveUrl('~/images/add.gif') + '" /></a>' + columnRowAdder + '</th>';
        } else {
            headerRow += '<th>&nbsp;</th>';
        }

        if (this.allowPivoting) {
            var aggregateDropdown = '';
            var expandByColDropdown = '';
            if (this.allowPivoting) {
                aggregateDropdown = this.htmlForAggregateDropdown();
                expandByColDropdown = this.htmlForExpandByColDropdown(this.getDataSource('raw'));
            }

            // prepend the pivoting controls to the header
            headerRow = "<tr><th style='text-align:left' colspan='" + (colCount + 1) + "'>" + aggregateDropdown + "&nbsp;&nbsp;" + expandByColDropdown + "</th></tr><tr>" + headerRow + "</tr>";
        } else {
            headerRow = '<tr>' + headerRow + '</tr>';
        }




        return headerRow;
    },

    htmlForColumnPrefPopup: function(col, visibleColCount, sortDir, dirDesc) {
        var ret = "<div title='Preferences for " + col.caption + "' class='columnPref pivotPopup " + (col.prefsPopupVisible ? "show " : "") + " col" + col.ordinal + "' onclick='javascript:return pivotView.noop(event);'>" + this.loadLabel('lblPreferences', 'Preferences for') + " " + col.caption + "<br />";
        col.prefsPopupVisible = false;
        if (visibleColCount > 1) {
            ret += this.loadLabel('lblHide', 'Hide') + " <a class='jsanchor' href='javascript:Hide Column' onclick='javascript:return pivotView.toggleColumn(\"" + col.columnName + "\").refresh().noop(event);' title='Hide " + col.caption + " Column'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt='remove' /></a><br />";
        }
        if (this.allowSorting) {
            ret += this.htmlForSortImage(col, sortDir, dirDesc);
        }
        if (this.allowFiltering) {
            ret += this.htmlForFilterEditor(col, false);
        }

        ret += "</div>";

        return ret;
    },


    htmlForSortImage: function(col, sortDir, dirDesc) {
        var ret = '';
        if (col.sortIndex > -1) {
            ret += this.loadLabel('lblSort', 'Sort') + " <a class='jsanchor' href='javascript:Change Sort Direction' onclick='javascript:return pivotView.toggleSortDir(\"" + col.columnName + "\", false).refresh().noop(event);' title='Sort by " + col.caption + " " + dirDesc + "'><img src='" + this.resolveUrl('~/images/' + sortDir + '.gif') + "' alt='" + sortDir + "' /></a> <a class='jsanchor' href='javascript:Remove Sort By' title='Do not sort by " + col.caption + "' onclick='javascript:return pivotView.toggleSort(\"" + col.columnName + "\").refresh().noop(event);'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt='remove' /></a><br />";
        } else {
            ret += this.loadLabel('lblSort', 'Sort') + " <a class='jsanchor' href='javascript:Sort By Column' onclick='javascript:return pivotView.toggleSort(\"" + col.columnName + "\").refresh().noop(event);' title='Sort by " + col.caption + "'><img src='" + this.resolveUrl('~/images/add.gif') + "' alt='sort' /></a><br />";
        }
        return ret;
    },

    htmlForFilterEditor: function(col, showAsPopup, title) {

        var ret = '';

        if (!col.filterText) {
            col.filterText = '';
        }

        var filterText = '';
        if (col.isFiltered) {
            filterText = col.filterText;
        }

        ret += "<div class='filterOn' title='Filter " + col.caption + "'><nobr>";
        if (title) {
            ret += title + " " + this.loadLabel('lblFilter', 'Filter') + ":<br />";
        } else {
            ret += this.loadLabel('lblFilter', 'Filter') + ": ";
        }

        if (col.isFiltered) {
            ret += "<input type='text' title='Filter value is case-insensitive' value='" + filterText + "' onkeypress='javascript: if((event.keyCode || event.which) == 13) { return pivotView.changeFilterText(" + col.ordinal + ", this.value).refresh().noop(event); } else { return true; }' class='filterTextBox col" + col.ordinal + "' width='30' maxlength='250' /> ";
            ret += "<a title='Remove Filter' class='jsanchor' href='javascript:Remove Filter' onclick='javascript:return pivotView.toggleFilter(" + col.ordinal + ", \"\", false, false).refresh().noop(event);'><img src='" + this.resolveUrl('~/images/remove.gif') + "' alt='remove' /></a>";
        } else {
            ret += "<input type='text' title='Filter value is case-insensitive' value='" + filterText + "' onkeypress='javascript: if((event.keyCode || event.which) == 13) { if (this.value.length == 0) { alert(\"You must enter a value to filter by.\"); this.focus(); return false; } else { return pivotView.changeFilterText(" + col.ordinal + ", this.value).refresh().noop(event); } } else { return true; }' class='filterTextBox col" + col.ordinal + "' width='30' maxlength='250' /> ";
            ret += "<a title='Add Filter' class='jsanchor' href='javascript:Add Filter' onclick='javascript:var sib = $(this).siblings(\"input[type=\\\"text\\\"]\"); if (sib.val().length == 0) { alert(\"You must enter a value to filter by.\"); sib.focus(); return false; } else { return pivotView.toggleFilter(" + col.ordinal + ", sib.val(), false, false).refresh().noop(event);}'><img src='" + this.resolveUrl('~/images/add.gif') + "' alt='add' /></a>";
        }


        ret += "</nobr></div>";


        if (showAsPopup) {
            ret = "<div class='filterEditor pivotPopup col" + col.ordinal + " hide' onclick='javascript:pivotView.noop(event);'>" + ret + "</div>";
        }


        return ret;
    },

    recalcPagingInfo: function(data) {

        this.pageCount = parseInt((data.rows.length - data.hiddenRowsCount) / this.pageSize, 10);
        while ((data.rows.length - data.hiddenRowsCount) > this.pageCount * this.pageSize) {
            // one got lopped off the end since it was a partial page.  add it back in.
            this.pageCount++;
        }

        if (this.pageIndex >= this.pageCount) {
            this.pageIndex = this.pageCount - 1;
        }
        if (this.pageIndex < 0) {
            this.pageIndex = 0;
        }

    },

    refresh: function(selector) {
        if (selector) {
            alert('selector specified!!!');
        }

        //alert('refreshing');

        this.busy();


        // we always start at the raw data level (for now)		
        this.dataOrMode = 'raw';

        // get the data object
        var data = this.getDataSource();
        if (!data) {
            // haven't pulled any data yet. just mark us as done.
            this.done();
            return this;
        }

        // group data as needed
        // (if grouping is applied, a sort is auto-applied to the grouping column as the most important sort index)
        data = this.groupData(data);

        // sort data as needed
        data = this.sortData(data);

        // filter data as needed
        data = this.filterData(data);

        // pivot data if needed
        data = this.pivotData(data);


        // so the dropdown doesn't go all whacky, remember the previous page size before we recalc
        var selectedPageSize = this.pageSize;

        // adjust page index / size / count as needed			
        this.recalcPagingInfo(data);


        // start header portion

        var selectors = "<span><b>" + this.loadLabel('lblSelect', 'Select:') + "</b>&nbsp;<a class='jsanchor' title='Select All Rows' href='javascript:Select All Rows' onclick='javascript:return pivotView.toggleAllCheckBoxes(null, \"on\").noop(event);'>" + this.loadLabel('lblAll', 'All') + "</a>,&nbsp;";
        selectors += "<a class='jsanchor' title='Select No Rows' href='javascript:Select No Rows' onclick='javascript:return pivotView.toggleAllCheckBoxes(null, \"off\").noop(event);'>" + this.loadLabel('lblNone', 'None') + "</a>,&nbsp;";
        selectors += "<a class='jsanchor' title='Toggle Selection of Every Row' href='javascript:Toggle Selection of Every Row' onclick='javascript:return pivotView.toggleAllCheckBoxes(null, \"inverse\").noop(event);'>" + this.loadLabel('lblInverse', 'Inverse') + "</a>,&nbsp;";
        selectors += "<a class='jsanchor' title='Select Highlighted Rows' href='javascript:Select Highlighted Rows' onclick='javascript:return pivotView.toggleAllCheckBoxes(null, \"hilite\").noop(event);'>" + this.loadLabel('lblHighlighted', 'Highlighted') + "</a>";
        selectors += "</span>";

        var pivotOptions = "";
        if (this.allowSorting || this.allowFiltering) {
            pivotOptions += this.htmlForOptionsPopup(data);
        }

        var pager = "";
        if (this.allowPaging) {
            pager = this.htmlForPager(data, selectedPageSize);
        }

        var exporter = "";
        if (this.allowExporting) {
            exporter = this.htmlForExportingPopup(data);
        }

        var htmlForBody = this.htmlForTableBody(data);

        var newHtml = "";

        if (this.getDataSource('raw').rows.length == 0) {
            // raw data source is empty, meaning there will never be any data
            newHtml = "<div class='pivoterEmpty'>" + this.emptyDataMessage + "</div>";
            this.done();
        } else {
            // create the header

            //var s = "<div>" + aggregateDropdown + "&nbsp;&nbsp;" + expandByColDropdown + "&nbsp;&nbsp;" + pivotOptions + "&nbsp;&nbsp;" + pager + "&nbsp;&nbsp;" + exporter + "</div>";
            newHtml = "<div>" + selectors + "&nbsp;&nbsp;&nbsp;&nbsp;" + pivotOptions + "&nbsp;&nbsp;" + pager + "&nbsp;&nbsp;" + exporter + "</div>";


            // start table portion
            newHtml += "<table class='grid' cellpadding='0' cellspacing='0'>";

            // create table header
            newHtml += this.htmlForHeaderRow(data);

            // create table body
            if (htmlForBody == "") {
                // raw data source is not empty, but current filter / grouping / pivot / etc is
                newHtml += "<tr><td colspan='100'><div class='pivoterEmpty'>" + this.emptyDataMessage + "</div></td></tr>";
            } else {
                newHtml += htmlForBody;
            }

            // end the table, tack on the footer
            newHtml += "</table>";
            newHtml += pager;
        }



        // display the new html
        $(this.selector).html(newHtml);

        // rebind events for the new html
        this.rebindEvents();

        return this;
    }


};


var pivotView = new pivoter();
