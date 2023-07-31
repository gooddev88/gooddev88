function sendUrlToPrint(url) {
    var beforeUrl = 'intent:';
    var afterUrl = '#Intent;';
    // Intent call with component
    afterUrl += 'component=ru.a402d.rawbtprinter.activity.PrintDownloadActivity;'
    afterUrl += 'package=ru.a402d.rawbtprinter;end;';
    document.location = beforeUrl + encodeURI(url) + afterUrl;
    return false;
}
// jQuery: set onclick hook for css class print-file
$(document).ready(function () {
    $('.print-file').click(function () {
        return sendUrlToPrint($(this).attr('href'));
    });
});