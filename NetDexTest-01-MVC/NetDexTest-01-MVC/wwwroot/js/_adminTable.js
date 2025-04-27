"use strict";

//fetch('{URL string}', { options })
//    .then({ callback function})   // Extract the data from the response
//    .then({ callback function})   // Do something with the data
//    .catch({ callback function}); // Handle any errors


document.getElementById("showIdButton").addEventListener(
    "click",
    () => {
        document.getElementById("hideIdButton").hidden = false;
        document.getElementById("showIdButton").hidden = true;
    },
    false,
);

document.getElementById("hideIdButton").addEventListener(
    "click",
    () => {
        document.getElementById("showIdButton").hidden = false;
        document.getElementById("hideIdButton").hidden = true;
    },
    false,
);