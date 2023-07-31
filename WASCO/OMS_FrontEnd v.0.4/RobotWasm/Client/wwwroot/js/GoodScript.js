function hardReload() {
    location.reload(true);
}
function alertx() {
    alert("Hello! I am an alert box!!");
}
// helper.ts 
// (this TypeScript source file will be trancepiled to "helper.js")

function downloadFromUrl(options: { url: string, fileName?: string }): void {
    const anchorElement = document.createElement('a');
    anchorElement.href = options.url;
    anchorElement.download = options.fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}


async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}


function get_os() {
    let os = "Unknown";

    if (navigator.appVersion.indexOf("Win") != -1) os = "Windows";
    if (navigator.appVersion.indexOf("Mac") != -1) os = "MacOS";
    if (navigator.appVersion.indexOf("X11") != -1) os = "UNIX";
    if (navigator.appVersion.indexOf("Linux") != -1) os = "Linux";
    if (navigator.appVersion.indexOf("Linux") != -1) os = "Linux";
    return os;
}
function isiOS() {
    return [
        'iPad Simulator',
        'iPhone Simulator',
        'iPod Simulator',
        'iPad',
        'iPhone',
        'iPod'
    ].includes(navigator.platform)
    // iPad on iOS 13 detection
    //|| (navigator.userAgent.includes("Mac") && "ontouchend" in document)
}
 