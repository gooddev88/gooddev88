window.NavigationManagerExtensions = {};
window.NavigationManagerExtensions.openInNewWindow = (url, message) => {
    var newTab = window.open('', '_blank');
    newTab.document.write(message);
    newTab.location.href = url;

}



function openTab(url) {
    // Create link in memory
    var a = window.document.createElement("a");
    a.target = '_blank';
    a.href = url;

    // Dispatch fake click
    var e = window.document.createEvent("MouseEvents");
    e.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
    a.dispatchEvent(e);
};

function DisableBack() {
    window.history.pushState(null, "", window.location.href);
    window.onpopstate = function () {
        window.history.pushState(null, "", window.location.href);
    };
}
 
function openPdf(url)
    {
     var omyFrame = document.getElementById("myFrame");
    omyFrame.style.display="block";
    omyFrame.src = url;
  }
 