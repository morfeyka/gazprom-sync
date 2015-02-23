var Layout = {};

Layout.Init = function() {
    Layout.MainSplitter_Resize();
    $(window).resize(Layout.MainSplitter_Resize);
}

Layout.MainSplitter_Resize = function() {
    var height = $(window).height();
    var splitter = $('#MainSplitter');
    splitter.height(height - 25);
    splitter.resize();
}

Layout.InnerSplitter_OnResize = function(e) {
    _messageBroker.Notify("InnerSplitterResizeEvent");
}