var Index = {};

Index.Init = function () {
    Index.Resize();
    $(window).resize(Index.Resize);
    _messageBroker.AddSubscriber("InnerSplitterResizeEvent", function () { Index.Resize(); });
    $('#IndexTabStrip').data('tTabStrip').removeTab(0);
    //    $.ajax({
    //        url: '/Home/GetTabThree/',
    //        contentType: 'application/html; charset=utf-8',
    //        type: 'GET',
    //        dataType: 'html'
    //    })
    //        .success(function(result) {

    //        });
};

Index.Resize = function() {
    var contentPane = $($('#InnerSplitter').children()[2]);
    var height = contentPane.height();
    var width = contentPane.width();

    // Resize the tab control to fit in the contract splitter.
    var tabStripCtrl = $('#IndexTabStrip');
    tabStripCtrl.height(height - 2);
    tabStripCtrl.width(width - 2);

    $('#tabOneContents').height(tabStripCtrl.height() - 50);
    $('#tabTwoContents').height(tabStripCtrl.height() - 50);
    $('#loadUsingAjax').height(tabStripCtrl.height() - 50);
};
