"use strict";

// use this class for DOM Manipulation functions
export let DOM = {
    createTextTD: (text) => {
        const td = document.createElement("td");
        td.appendChild(document.createTextNode(text));
        return td;
    },

    createHiddenTextTD: (text) => {
        var td = document.createElement("td");

        td.appendChild(document.createTextNode(text));
        //td.hidden = true;
        td.classList.add('d-none');
        td.classList.add('adminHidden');

        return td;
    },


    createULfromArrayPropertyTD: (array, property) => {
        const td = document.createElement("td");
        //const table = document.createElement('table');
        const ul = document.createElement('ul');
        let prop = `${property}`;

        array.forEach(arrayItem => {
            //const row = table.insertRow();
            const li = document.createElement('li');
            li.appendChild(document.createTextNode(arrayItem[prop]));
            ul.appendChild(li);
        });

        td.appendChild(ul);
        return td;
    },



    setElementText: (elementId, text) => {
        const element = document.querySelector(elementId);
        element.appendChild(document.createTextNode(text));
    },

    removeChildren: (element) => {
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
    },


    // CHECKPOINT 8 : createLink()
    createButtonLink: (text, url, btnType) => {
        let tempBtnType = 'basic disabled';
        if (btnType == 'primary' || btnType == 'warning' || btnType == 'info' || btnType == 'danger') {
            tempBtnType = `${btnType}`;
        }
        else {
            console.error('JS ERROR: btnType must be primary, warning, info, or danger');
            console.log('JS ERROR: btn will be set to basic disabled');
        }

        const a = document.createElement("a");
        a.setAttribute("href", url);
        a.setAttribute("class", `btn btn-${tempBtnType} mx-1`);
        a.appendChild(document.createTextNode(text));
        return a;

    }





}