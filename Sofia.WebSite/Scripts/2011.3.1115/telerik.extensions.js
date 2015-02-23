/*
* @Copyright (c) 2011 John DeVight
* Permission is hereby granted, free of charge, to any person
* obtaining a copy of this software and associated documentation
* files (the "Software"), to deal in the Software without
* restriction, including without limitation the rights to use,
* copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following
* conditions:
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
* OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
* HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
* WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.
*/

/// <summary>
/// Extend the Telerik Extensions for ASP.NET MVC.
/// </summary>
(function($){
    // Was the tekerik.window.min.js added to the page by the telerik script registrar?
    if ($.telerik.window != undefined) {
        // Extend the window plugin.
        var windowExtensions = {
            /// <summary>
            /// Set a new height for the window.
            /// </summary>
            /// <param type="int" name="h">New height for the window.</param>
            setHeight: function(h) {
                $(this.element).find('.t-window-content').height(h);
            },
            /// <summary>
            /// Set a new width for the window.
            /// </summary>
            /// <param type="int" name="w">New width for the window.</param>
            setWidth: function(w) {
                $(this.element).find('.t-window-content').width(w);
            }
        };

        // Add the extensions to the window plugin.
        $.extend(true, $.telerik.window.prototype, windowExtensions);
    }



    // Was the tekerik.tabstrip.min.js added to the page by the telerik script registrar?
    if ($.telerik.tabstrip != undefined) {
        // Extend the tabstrip plugin.
        var tabstripExtensions = {
            /// <summary>
            /// Get a tab.
            /// </summary>
            /// <param type="json object" name="o">json object with either the text or the index of the tab.</param>
            /// <return>jQuery object of the tab [li.t-item]</return>
            /// <example>
            /// var tab = $('#MyTabStrip').data('tTabStrip').getTab({ text: 'Tab 2' })
            /// var tab = $('#MyTabStrip').data('tTabStrip').getTab({ index: 1 })
            /// </example>
            getTab: function(o) {
                var tab = null;
                if (o.text != null) {
                    var tab = $(this.element).find('.t-item').find("a:contains('" + o.text + "')").parent();
                }
                else if (o.index != null) {
                    tab = $($(this.element).find('.t-item')[o.index]);
                }
                return tab;
            },
            /// <summary>
            /// Hide a tab.
            /// </summary>
            /// <param type="json object" name="o">json object with either the text or the index of the tab.</param>
            /// <example>
            /// $('#MyTabStrip').data('tTabStrip').hideTab({ text: 'Tab 2' })
            /// $('#MyTabStrip').data('tTabStrip').hideTab({ index: 1 })
            /// </example>
            hideTab: function(o) {
                var tab = this.getTab(o);
                if (tab != null) {
                    tab.css('visibility', 'hidden');
                    tab.css('display', 'none');
                }
            },
            /// <summary>
            /// Change the tab text.
            /// </summary>
            /// <param type="json object" name="o">json object with either the text or the index of the tab.</param>
            /// <example>
            /// $('#MyTabStrip').data('tTabStrip').showTab({ text: 'Tab 2' })
            /// $('#MyTabStrip').data('tTabStrip').showTab({ index: 1 })
            /// </example>
            showTab: function(o) {
                var tab = this.getTab(o);
                if (tab != null) {
                    tab.css('visibility', '');
                    tab.css('display', '');
                }
            },
            /// <summary>
            /// Select a tab.
            /// </summary>
            /// <param type="json object" name="o">json object with either the text or the index of the tab.</param>
            /// <example>
            /// $('#MyTabStrip').data('tTabStrip').selectTab({ text: 'Tab 2' })
            /// $('#MyTabStrip').data('tTabStrip').selectTab({ index: 1 })
            /// </example>
            selectTab: function(o) {
                var tab = this.getTab(o);
                if (tab != null) {
                    this.select(tab);
                }
            },
            /// <summary>
            /// Show a tab.
            /// </summary>
            /// <param type="json object" name="o">json object with either the text or the index of the tab and the newText for the tab.</param>
            /// <example>
            /// $('#MyTabStrip').data('tTabStrip').setTabText({ text: 'Tab 2', newText: 'Second Tab' })
            /// $('#MyTabStrip').data('tTabStrip').setTabText({ index: 1, newText: 'Second Tab' })
            /// </example>
            setTabText: function(o) {
                var tab = this.getTab(o);
                if (tab != null) {
                    tab.find('a').text(o.newText);
                }
            },
            /// <summary>
            /// Add a tab.
            /// </summary>
            /// <param type="json object" name="t">json object with text and html for the tab.</param>
            /// <example>
            /// $.ajax({
            ///     url: '/Home/GetTabThree/',
            ///     contentType: 'application/html; charset=utf-8',
            ///     type: 'GET',
            ///     dataType: 'html'
            /// })
            /// .success(function(result) {
            ///     $('#MyTabStrip').data('tTabStrip').addTab({ text: 'Tab Three', html: result });
            /// });
            /// </example>
            addTab: function(t) {
                var tabstrip = $(this.element);
                var tabstripitems = tabstrip.find('.t-tabstrip-items');
                var cnt = tabstripitems.children().length;
                var tabname = tabstrip.attr('id');

                tabstripitems.append(
                    $('<li />')
                        .addClass('t-item')
                        .addClass('t-state-default')
                        .append(
                            $('<a />')
                                .attr('href', '#' + tabname + '-' + (cnt + 1))
                                .addClass('t-link')
                                .text(t.text)
                        )
                    );

                var $contentElement =
                    $('<div />')
                        .attr('id', tabname + '-' + (cnt + 1))
                        .addClass('t-content')
                        .append(t.html)

                tabstrip.append($contentElement);

                tabstrip.data('tTabStrip').$contentElements.push($contentElement[0]);
            },
            /// <summary>
            /// Remove a tab.
            /// </summary>
            /// <param type="int" name="i">index of the tab to remove.</param>
            /// <example>
            /// $('#MyTabStrip').data('tTabStrip').removeTab(1);
            /// </example>
            removeTab: function(i) {
                var tabstrip = $(this.element);
                var tabname = tabstrip.attr('id');
                var tabstripitems = tabstrip.find('.t-tabstrip-items');

                // There must be atleast two tabs to remove a tab.
                if (tabstripitems.children().length >= 1) {
                    var tab = this.getTab({ index: i });
                    // If the active tab is being removed, set another tab as active.
                    if (tab.hasClass('t-state-active') == true) {
                        var j = i == 0 ? 1 : (i - 1);
                        this.activateTab(this.getTab({ index: j }));
                    }
                    tab.remove();

                    // Remove the tab contents.
                    $(tabstrip.children()[i + 1]).remove();
                    tabstrip.data('tTabStrip').$contentElements.splice(i, 1);

                    // Rename the tab href.
                    $.each(tabstripitems.children(), function(idx, tab) {
                        $($(tab).children()[0]).attr('href', '#' + tabname + '-' + (idx + 1));
                    });

                    // Rename tab contents.
                    $.each(tabstrip.children(), function(idx, contentElement) {
                        if($(contentElement).is('div')) {
                            $(contentElement).attr('id', tabname + '-' + idx);
                        }
                    })
                }
            }
        };

        // Add the extensions to the tabstrip plugin.
        $.extend(true, $.telerik.tabstrip.prototype, tabstripExtensions);
    }



    // Was the tekerik.grid.min.js added to the page by the telerik script registrar?
    if ($.telerik.grid != undefined) {
        // Extend the grid plugin.
        var gridExtensions = {
            /// <summary>
            /// Hide a column in the grid.
            /// </summary>
            /// <param type="int" name="i">Zero based index for the column.</param>
            /// <example>
            /// $('#MyGrid').data('tTabStrip').hideColumn(1);
            /// </example>
            hideColumn: function(i) {
                var table = $(this.element).find('table');

                if (table.find('thead tr th').length > i) {
                    $($(table).find('thead tr th')[i]).css('display', 'none');
                }

                var rows = $(table).find('tbody tr');
                if (rows.length >= 1 && $(rows[0]).find('td').length > i) {
                    $.each(rows, function(idx, row) {
                        $($(row).find('td')[i]).css('display', 'none');
                    });
                }
            },
            /// <summary>
            /// Show a column in the grid.
            /// </summary>
            /// <param type="int" name="i">Zero based index for the column.</param>
            /// <example>
            /// $('#MyGrid').data('tTabStrip').showColumn(1);
            /// </example>
            showColumn: function(i) {
                var table = $(this.element).find('table');

                if (table.find('thead tr th').length > i) {
                    $($(table).find('thead tr th')[i]).css('display', '');
                }

                var rows = $(table).find('tbody tr');
                if (rows.length >= 1 && $(rows[0]).find('td').length > i) {
                    $.each(rows, function(idx, row) {
                        $($(row).find('td')[i]).css('display', '');
                    });
                }
            }
        };

        // Add the extensions to the grid plugin.
        $.extend(true, $.telerik.grid.prototype, gridExtensions);
    }
})(jQuery);