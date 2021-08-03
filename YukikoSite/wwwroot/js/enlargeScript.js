const sampleImg = document.getElementsByTagName("img")[0];
const sampleStyle = sampleImg.style;
var currImg;
function enlargeImg(img, scaleValue = 3) {
    if (isMobile())
        return;

    if (currImg === img) {
        img.style = sampleStyle;
        currImg = undefined;
    }
    else {
        if (currImg != undefined)
            currImg.style = sampleStyle;

        img.style.position = "fixed";
        img.style.top = "50%";
        img.style.left = "50%";
        img.style.transform = "translate(-50%, -50%) scale(" + scaleValue + ")";
        currImg = img;
    }
}
function isMobile() {
    return screen.width * 0.6 < sampleImg.width;
}