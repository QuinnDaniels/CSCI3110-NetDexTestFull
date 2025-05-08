"use strict";


function _formatDateMMDDYYYY(dateString) {

    // equivalent to:
    // const formatDateFancy = (dateString) => new Date(dateString).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' });
    //
    // Above line creates MM/DD/YYYY
    // To Create MM-DD-YYYY:
    //
    // const formatDateFancyDash = (dateString) =>
    //  new Date(dateString).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' }).replace(/\//g, '-');

    const date = new Date(dateString);
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${month}-${day}-${year}`;
}

function _formatDateDDMMYYYY(dateString) {
    const date = new Date(dateString);
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
}


function _formatDateYYYYMMDD(dateString) {
    const date = new Date(dateString);
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${year}-${month}-${day}`;
}

// CHATGPT
function _calculateAge(dateString) {
    const birthDate = new Date(dateString);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    const dayDiff = today.getDate() - birthDate.getDate();
    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
        age--; // Birthday hasn't happened yet this year
    }
    return age;
}

function _calculateAgeXX(dateString) {
    const birthDate = new Date(dateString);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    const dayDiff = today.getDate() - birthDate.getDate();
    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
        age--; // Birthday hasn't happened yet this year
    }
    if (!(Math.abs(age) > 10)) {
        if (age < 0) { age = `-0${Math.abs(age)}`; }
        else { age = `0${age}`; }
    }
    return age;
}

function _isOnlySpacesCheck(str) {
    return str.trim().length === 0;
}
function _isNullOrWhitespaceCheck(str) {
    return (str === null || str === undefined || str.trim() === '');
}

// check if a string is invalid.
// if string is invalid, return a placeholder string.
// if string is valid, return the string
function _formatStrDisplay(str) {
    if (_isNullOrWhitespaceCheck(str)) {
        return "---";
    }
    else {
        return str;
    }
}

function _isDate(value) {
    return value instanceof Date && !isNaN(value);
}

function _formatRatingAsFiveStars(rate) {
    const rateMax = 5;
    const rateMin = 0;
    let maxxer = rateMax + 1;
    //const rate = -5;
    //let rater = rate - 1;
    let invRate = rateMax - rate;

    let text = "";
    if ((rate > rateMax) || (rate < rateMin)) {
        if (rate > rateMax) { text = "&#11212; &#11212; &#11212; &#11212; &#11212;"; }
        if (rate < rateMin) { text = "&#11199; &#11199; &#11217; &#11199; &#11199;"; }
    }
    else {
        for (let i = 1; i < maxxer - invRate; i++) {
            text += "&#9733;";
        }
        for (let i = 1; i < maxxer - rate; i++) {
            text += "&#9734;";
        }
    }
    return text;
}




// use this class for DOM Manipulation functions
export let DOM = {

    // TODO Format DateTime as DD-MM-YYYY
    // TODO Format DateTime as MM-DD-YYYY


    createTextTD: (text) => {
        const td = document.createElement("td");
        td.appendChild(document.createTextNode(text));
        return td;
    },


    logElementToConsole: (value) => {
        //alert(value);
        console.log(value);
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


    createULfromArrayProperty: (array, property) => {
        //const td = document.createElement("td");
        //const table = document.createElement('table');
        const ul = document.createElement('ul');
        let prop = `${property}`;

        array.forEach(arrayItem => {
            //const row = table.insertRow();
            console.log(`${property}`, `${arrayItem[prop]}`);
            const li = document.createElement('li');

            // TODO issues here with DateStrings? Revisit after semester end
            li.appendChild(document.createTextNode(arrayItem[prop]));
            ul.appendChild(li);
        });

        //td.appendChild(ul);
        return ul;
    },

    setDirectElementText: (element, text) => {
        //const element = document.querySelector(elementId);
        element.appendChild(document.createTextNode(text));
    },

    setDirectElementTextWithPlaceholder: (element, text) => {
        //const element = document.querySelector(elementId);
        var tempText = text;
        if (tempText == "") { tempText = "---" }
        element.appendChild(document.createTextNode(tempText));
    },


    setElementText: (elementId, text) => {
        const element = document.querySelector(elementId);
        element.appendChild(document.createTextNode(text));
    },

    setElementValue: (elementId, value) => {
        var modifier = "";
        if (value == undefined) {
            value = "";
            console.info(`setting element ${elementId} to blank`, value);
        }
        try {
            if (_isDate(value)) {
                console.info(`Date Value detected! attempting conversion`, value);
                value = _formatDateYYYYMMDD(value)
                console.log(value);
            }
        }
        catch (e) {
            console.info(e);
        }
        finally {

            console.log(elementId, value);
            const element = document.querySelector(elementId);
            element.value = value;
        }
    },

    removeChildren: (element) => {
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
    },

    createImg: (src, alt) => {
        const img = document.createElement("img");
        img.setAttribute("src", src);
        img.setAttribute("alt", alt);
        return img;
    },
    createImageTR: (src, alt) => {
        const tr = document.createElement("tr");
        const td = DOM.createImageTD(src, alt);
        td.setAttribute("colspan", "4");
        tr.appendChild(td);
        return tr;
    },

    createImageTD: (src, alt) => {
        const td = document.createElement("td");
        const img = DOM.createImg(src, alt);
        td.appendChild(img);
        return td;
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

    },



    //createDexPersonRow: (tbody, PersonDexListVM) =>
    //{
    //    const tr = document.createElement("tr");
    //    const td = document.createElement("td");
    //    tbody.appendChild(tr);
    //    const span = document.createElement("span");
    //    const sup = document.createElement("sup");
    //    const sub = document.createElement("sub");

    //    //.ClassList.add("regionalNumberCell");
    //    //.ClassList.add("personIdField");
    //    //.ClassList.add("text-decoration-underline");
    //    let tdRegional = document.createElement("td");




    //    //const ul = document.createElement('ul');
    //    //let prop = `${property}`;

    //    //array.forEach(arrayItem => {
    //    //    //const row = table.insertRow();
    //    //    console.log(`${property}`, `${arrayItem[prop]}`);
    //    //    const li = document.createElement('li');
    //    //    li.appendChild(document.createTextNode(arrayItem[prop]));
    //    //    ul.appendChild(li);
    //    //});


    //},

    // CHATGPT
    formatDateMMDDYYYY: (dateString) => {

        // equivalent to:
        // const formatDateFancy = (dateString) => new Date(dateString).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' });
        //
        // Above line creates MM/DD/YYYY
        // To Create MM-DD-YYYY:
        //
        // const formatDateFancyDash = (dateString) =>
        //  new Date(dateString).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' }).replace(/\//g, '-');

        const date = new Date(dateString);
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        const year = date.getFullYear();
        return `${month}-${day}-${year}`;
    },

    formatDateDDMMYYYY: (dateString) => {
        const date = new Date(dateString);
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
    },

    // CHATGPT
    calculateAge: (dateString) => {
        const birthDate = new Date(dateString);
        const today = new Date();
        let age = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();
        const dayDiff = today.getDate() - birthDate.getDate();
        if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
            age--; // Birthday hasn't happened yet this year
        }
        return age;
    },

    calculateAgeXX: (dateString) => {
        const birthDate = new Date(dateString);
        const today = new Date();
        let age = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();
        const dayDiff = today.getDate() - birthDate.getDate();
        if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
            age--; // Birthday hasn't happened yet this year
        }
        if (!(Math.abs(age) > 10))
        {
            if (age < 0) { age = `-0${Math.abs(age)}`; }
            else { age = `0${age}`; }
        }
        return age;
    },

    isOnlySpacesCheck: (str) => {
        return str.trim().length === 0;
    },
    isNullOrWhitespaceCheck: (str) => {
        return (str === null || str === undefined || str.trim() === '');
    },

    // check if a string is invalid.
    // if string is invalid, return a placeholder string.
    // if string is valid, return the string
    formatStrDisplay: (str) => {
        if (isNullOrWhitespaceCheck(str)) {
            return "---";
        }
        else {
            return str;
        }
    },


    formatRatingAsFiveStars: (rate) => {
        const rateMax = 5;
        const rateMin = 0;
        let maxxer = rateMax + 1;
        //const rate = -5;
        //let rater = rate - 1;
        let invRate = rateMax - rate;

        let text = "";
        if ((rate > rateMax) || (rate < rateMin)) {
            if (rate > rateMax) { text = "&#11212;&#11212;&#11212;&#11212;&#11212;"; }
            if (rate < rateMin) { text = "&#11199;&#11199;&#11217;&#11199;&#11199;"; }
        }
        else {
            for (let i = 1; i < maxxer - invRate; i++) {
                text += '&starf;';
            }
            for (let i = 1; i < maxxer - rate; i++) {
                text += "&star;";
            }
        }
        return text;
    },


    // CHATGPT
    createTemplateRowPersonList: () => {
        const tr = document.createElement('tr');
        tr.setAttribute('id', 'templateRow');
        //tr.classList.add('d-none');

        // First td - regionalNumberCell
        const tdRegional = document.createElement('td');
        tdRegional.classList.add('regionalNumberCell');

        const spanOuter = document.createElement('span');

        const spanPersonId = document.createElement('span');
        spanPersonId.classList.add('personIdField');

        const supPersonId = document.createElement('sup');
        supPersonId.textContent = '10';
        spanPersonId.appendChild(supPersonId);

        const spanNo = document.createElement('span');
        spanNo.innerHTML = `N<sub class="text-decoration-underline">o.</sub> 404`;

        spanOuter.appendChild(spanPersonId);
        spanOuter.appendChild(spanNo);
        tdRegional.appendChild(spanOuter);
        tr.appendChild(tdRegional);

        // Second td - hidden DexId
        const tdDexId = document.createElement('td');
        tdDexId.hidden = true;
        tdDexId.classList.add('d-none');
        tdDexId.textContent = 'DexId';
        tr.appendChild(tdDexId);

        // Third td - favCell
        const tdFav = document.createElement('td');
        tdFav.classList.add('favCell');
        tdFav.style.fontWeight = 'bolder';
        tdFav.innerHTML = '&radic;';
        tr.appendChild(tdFav);

        // Fourth td - nicknameCell
        const tdNickname = document.createElement('td');
        tdNickname.classList.add('nicknameCell');
        tdNickname.textContent = 'Nickname';
        tr.appendChild(tdNickname);

        // Fifth td - firstNameCell
        const tdFirstName = document.createElement('td');
        tdFirstName.classList.add('firstNameCell');

        const spanFirstName = document.createElement('span');
        spanFirstName.textContent = 'Kalinka';

        const pFirstName = document.createElement('p');
        const subFirstName = document.createElement('sub');
        subFirstName.textContent = '"Ka-Lean-Ka"';
        pFirstName.appendChild(subFirstName);

        tdFirstName.appendChild(spanFirstName);
        tdFirstName.appendChild(pFirstName);
        tr.appendChild(tdFirstName);

        // Sixth td - middleNameCell
        const tdMiddleName = document.createElement('td');
        tdMiddleName.classList.add('middleNameCell');

        const spanMiddleName = document.createElement('span');
        spanMiddleName.textContent = 'Mega Man 4';

        const pMiddleName = document.createElement('p');
        const subMiddleName = document.createElement('sub');
        subMiddleName.textContent = '"MM4"';
        pMiddleName.appendChild(subMiddleName);

        tdMiddleName.appendChild(spanMiddleName);
        tdMiddleName.appendChild(pMiddleName);
        tr.appendChild(tdMiddleName);

        // Seventh td - lastNameCell
        const tdLastName = document.createElement('td');
        tdLastName.classList.add('lastNameCell');

        const spanLastName = document.createElement('span');
        spanLastName.textContent = 'Cossack';

        const pLastName = document.createElement('p');
        const subLastName = document.createElement('sub');
        subLastName.textContent = '"Koss-sack"';
        pLastName.appendChild(subLastName);

        tdLastName.appendChild(spanLastName);
        tdLastName.appendChild(pLastName);
        tr.appendChild(tdLastName);

        // Eighth td - birthdayField
        const tdBirthday = document.createElement('td');
        tdBirthday.classList.add('birthdayField');
        tdBirthday.textContent = '06-12-1991';
        tr.appendChild(tdBirthday);

        // Ninth td - gender info
        const tdGender = document.createElement('td');

        const spanGender = document.createElement('span');
        spanGender.textContent = 'Girl';

        const brGender = document.createElement('br');

        const spanPronouns = document.createElement('span');
        const subPronouns = document.createElement('sub');
        subPronouns.textContent = '(She/Her)';
        spanPronouns.appendChild(subPronouns);

        tdGender.appendChild(spanGender);
        tdGender.appendChild(brGender);
        tdGender.appendChild(spanPronouns);
        tr.appendChild(tdGender);

        // Tenth td - ratingField
        const tdRating = document.createElement('td');
        tdRating.classList.add('ratingField');
        tdRating.textContent = '5';
        tr.appendChild(tdRating);

        // Eleventh td - recordCollectorCell
        const tdRecordCollector = document.createElement('td');
        tdRecordCollector.classList.add('recordCollectorCell');

        const subEntries = document.createElement('sub');
        subEntries.style.textOrientation = 'upright';
        subEntries.textContent = ' Entries: ';

        const pEntries = document.createElement('p');
        pEntries.classList.add('entriesCountField');
        pEntries.textContent = '4';

        tdRecordCollector.appendChild(subEntries);
        tdRecordCollector.appendChild(pEntries);
        tr.appendChild(tdRecordCollector);

        // Twelfth td - contactInfoCell
        const tdContact = document.createElement('td');
        tdContact.classList.add('contactInfoCell');

        const subSocials = document.createElement('sub');
        subSocials.style.textOrientation = 'upright';
        subSocials.textContent = ' SocialMedias: ';

        const partialSocials = document.createElement('partial');
        partialSocials.classList.add('socialMediasCount');
        partialSocials.innerHTML = '8<p></p>';

        tdContact.appendChild(subSocials);
        tdContact.appendChild(partialSocials);
        tr.appendChild(tdContact);

        // Thirteenth td - personPersonCell
        const tdPersonPerson = document.createElement('td');
        tdPersonPerson.classList.add('personPersonCell');

        const spanSymbol = document.createElement('span');
        spanSymbol.innerHTML = '&#9720;&nbsp;&nbsp;';

        const spanParent = document.createElement('span');
        spanParent.classList.add('personParentField');
        spanParent.textContent = '2';

        const brPerson = document.createElement('br');

        const spanChildren = document.createElement('span');
        spanChildren.classList.add('personChildrenField');
        spanChildren.style.textDecoration = 'overline';
        spanChildren.textContent = '16';

        const spanEndSymbol = document.createElement('span');
        spanEndSymbol.style.textDecoration = 'overline';
        spanEndSymbol.innerHTML = '&nbsp;&nbsp;&#9727;';

        tdPersonPerson.appendChild(spanSymbol);
        tdPersonPerson.appendChild(spanParent);
        tdPersonPerson.appendChild(brPerson);
        tdPersonPerson.appendChild(spanChildren);
        tdPersonPerson.appendChild(spanEndSymbol);

        tr.appendChild(tdPersonPerson);

        // Fourteenth td - edit links
        const tdLinks = document.createElement('td');
        tdLinks.innerHTML = `<a href="/People/Edit">Edit</a> | 
                         <a href="/People/Details">Details</a> | 
                         <a href="/People/Delete">Delete</a>`;
        tr.appendChild(tdLinks);

        return tr;
    },

    // modified from CHATGPT
    createPersonDexListRow: (PersonDexListVM, input) => {
        const tr = document.createElement('tr');
        const pi = PersonDexListVM;
        tr.setAttribute('id', 'templateRow');
        //tr.classList.add('d-none');

        // First td - regionalNumberCell
        const tdRegional = document.createElement('td');
        tdRegional.classList.add('regionalNumberCell');

        const spanOuter = document.createElement('span');

        const spanPersonId = document.createElement('span');
        spanPersonId.classList.add('personIdField');

        const supPersonId = document.createElement('sup');
        console.log("LOG: DOM.createPersonRowDexList: Id: ", pi.Id);
        supPersonId.textContent = `${pi.Id}`;
        spanPersonId.appendChild(supPersonId);

        const spanNo = document.createElement('span');
        spanNo.innerHTML = `N<sub class="text-decoration-underline">o.</sub> ${pi.LocalCounter}`;

        spanOuter.appendChild(spanPersonId);
        spanOuter.appendChild(spanNo);
        tdRegional.appendChild(spanOuter);
        tr.appendChild(tdRegional);

        // Second td - hidden DexId
        const tdDexId = document.createElement('td');
        tdDexId.hidden = true;
        tdDexId.classList.add('d-none');
        tdDexId.textContent = `${pi.DexId}`;
        tr.appendChild(tdDexId);

        // Third td - favCell
        const tdFav = document.createElement('td');
        tdFav.classList.add('favCell');
        tdFav.style.fontWeight = 'bolder';

        if (pi.Favorite == true) {
            tdFav.innerHTML = '&#x1F5F9;'; //BALLOT BOX WITH BOLD CHECK (1,2)
        } else {
            tdFav.innerHTML = '&#x26F6;'; //SQUARE FOUR CORNERS (2)
        }

        tr.appendChild(tdFav);

        // Fourth td - nicknameCell
        const tdNickname = document.createElement('td');
        tdNickname.classList.add('nicknameCell');
        tdNickname.textContent = `${pi.Nickname}`;
        tr.appendChild(tdNickname);

        // Fifth td - firstNameCell
        const tdFirstName = document.createElement('td');
        tdFirstName.classList.add('firstNameCell');
        console.log("LOG: DOM.createPersonRowDexList: ", pi.NameFirst);
        console.log("LOG: DOM.createPersonRowDexList: ", _formatStrDisplay(pi.NameFirst));
        const spanFirstName = document.createElement('span');
        // FIXME TODO
        spanFirstName.textContent = _formatStrDisplay(pi.NameFirst);

        const pFirstName = document.createElement('p');
        const subFirstName = document.createElement('sub');
        subFirstName.textContent = `"${_formatStrDisplay(pi.PhFirstName)}"`;
        pFirstName.appendChild(subFirstName);

        tdFirstName.appendChild(spanFirstName);
        tdFirstName.appendChild(pFirstName);
        tr.appendChild(tdFirstName);

        // Sixth td - middleNameCell
        const tdMiddleName = document.createElement('td');
        tdMiddleName.classList.add('middleNameCell');

        const spanMiddleName = document.createElement('span');
        spanMiddleName.textContent = _formatStrDisplay(pi.NameMiddle);

        const pMiddleName = document.createElement('p');
        const subMiddleName = document.createElement('sub');
        subMiddleName.textContent = `"${_formatStrDisplay(pi.PhNameMiddle)}"`;
        pMiddleName.appendChild(subMiddleName);

        tdMiddleName.appendChild(spanMiddleName);
        tdMiddleName.appendChild(pMiddleName);
        tr.appendChild(tdMiddleName);

        // Seventh td - lastNameCell
        const tdLastName = document.createElement('td');
        tdLastName.classList.add('lastNameCell');

        const spanLastName = document.createElement('span');
        spanLastName.textContent = _formatStrDisplay(pi.NameLast);

        const pLastName = document.createElement('p');
        const subLastName = document.createElement('sub');
        subLastName.textContent = `"${_formatStrDisplay(pi.PhNameLast)}"`;
        pLastName.appendChild(subLastName);

        tdLastName.appendChild(spanLastName);
        tdLastName.appendChild(pLastName);
        tr.appendChild(tdLastName);

        // Eighth td - birthdayField
        const tdBirthday = document.createElement('td');
        tdBirthday.classList.add('birthdayCell');


        const spanBirthday = document.createElement('span');
        spanBirthday.classList.add('birthdayField');

            // convert DateTime format ("2025-04-28T20:25:56.2180603-04:00") to MMDDYYYY

        if (_isNullOrWhitespaceCheck(pi.DateOfBirth)) {
            spanBirthday.textContent = "xx-xx-xxxx";//formatStrDisplay(pi.DateOfBirth);
        }
        else {
            const birthdayFormatted = _formatDateMMDDYYYY(pi.DateOfBirth);
            spanBirthday.textContent = birthdayFormatted;
        }

        const brBirthday = document.createElement('br');
        const spanAge = document.createElement('span');
        spanAge.classList.add('ageField');

        if (_isNullOrWhitespaceCheck(pi.DateOfBirth)) {
            spanAge.textContent = "(?? y/o)";
        }
        else {
                // calculate Age in years from DateTime format ("2025-04-28T20:25:56.2180603-04:00")
            const calcAge = _calculateAgeXX(pi.DateOfBirth);
            spanAge.textContent = `(${calcAge} y/o)`; 
        }

        tdBirthday.appendChild(spanBirthday);
        tdBirthday.appendChild(brBirthday);
        tdBirthday.appendChild(spanAge);

        tr.appendChild(tdBirthday);


        // Ninth td - gender info
        const tdGender = document.createElement('td');

        const spanGender = document.createElement('span');

        //let genderOut = pi.Gender;
        //if (pi.Gender == null) { genderOut = "---" }
        //if (pi.Gender == "") { genderOut = "---" }
        spanGender.textContent = _formatStrDisplay(pi.Gender);

        const brGender = document.createElement('br');

        const spanPronouns = document.createElement('span');
        const subPronouns = document.createElement('sub');
        subPronouns.textContent = `(${_formatStrDisplay(pi.Pronouns)})`;
        spanPronouns.appendChild(subPronouns);

        tdGender.appendChild(spanGender);
        tdGender.appendChild(brGender);
        tdGender.appendChild(spanPronouns);
        tr.appendChild(tdGender);

        // Tenth td - ratingField
        const tdRating = document.createElement('td');
        tdRating.classList.add('ratingField');

        //TODO
        tdRating.innerHTML = _formatRatingAsFiveStars(pi.Rating);

        tr.appendChild(tdRating);

        // Eleventh td - recordCollectorCell
        const tdRecordCollector = document.createElement('td');
        tdRecordCollector.classList.add('recordCollectorCell');

        const subEntries = document.createElement('sub');
        subEntries.style.textOrientation = 'upright';
        subEntries.textContent = ' Entries: ';

        const pEntries = document.createElement('p');
        pEntries.classList.add('entriesCountField');
        pEntries.textContent = pi.RcEntryItemsCount;

        tdRecordCollector.appendChild(subEntries);
        tdRecordCollector.appendChild(pEntries);
        tr.appendChild(tdRecordCollector);

        // Twelfth td - contactInfoCell
        const tdContact = document.createElement('td');
        tdContact.classList.add('contactInfoCell');

        const subSocials = document.createElement('sub');
        subSocials.style.textOrientation = 'upright';
        subSocials.textContent = ' SocialMedias: ';

        const pSocials = document.createElement('p');
        pSocials.classList.add('socialMediasCount');
        pSocials.textContent = pi.CiSocialMediasCount;

        tdContact.appendChild(subSocials);
        tdContact.appendChild(pSocials);
        tr.appendChild(tdContact);

        // Thirteenth td - personPersonCell
        const tdPersonPerson = document.createElement('td');
        tdPersonPerson.classList.add('personPersonCell');

        const spanSymbol = document.createElement('span');
        spanSymbol.innerHTML = '&#9720;&nbsp;&nbsp;';

        const spanParent = document.createElement('span');
        spanParent.classList.add('personParentField');
        spanParent.textContent = pi.PersonParentsCount;

        const brPerson = document.createElement('br');

        const spanChildren = document.createElement('span');
        spanChildren.classList.add('personChildrenField');
        spanChildren.style.textDecoration = 'overline';
        spanChildren.textContent = pi.PersonChildrenCount;

        const spanEndSymbol = document.createElement('span');
        spanEndSymbol.style.textDecoration = 'overline';
        spanEndSymbol.innerHTML = '&nbsp;&nbsp;&#9727;';

        tdPersonPerson.appendChild(spanSymbol);
        tdPersonPerson.appendChild(spanParent);
        tdPersonPerson.appendChild(brPerson);
        tdPersonPerson.appendChild(spanChildren);
        tdPersonPerson.appendChild(spanEndSymbol);

        tr.appendChild(tdPersonPerson);
        console.log("LOG input DOM: ", input);

        // Fourteenth td - edit links
        const tdLinks = document.createElement('td');
        tdLinks.innerHTML = `<a href="/dex/u/${input}/edit/${pi.Nickname}">Edit</a> | 
                         <a href="/dex/u/${input}/p/${pi.LocalCounter}">Details</a> | 
                         <a href="/dex/u/${input}/delete/${pi.Nickname}">Delete</a>`;
        tr.appendChild(tdLinks);

        return tr;
    },

    personDetailsButtons: (input, criteria) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<a class="btn btn-outline-primary" href="/dex/u/${input}/edit/${criteria}">Edit</a> | 
             <a class="btn btn-outline-danger" href="/dex/u/${input}/delete/${criteria}">Delete</a> |
             <a class="btn btn-link" href="/dex/u/${input}">Back to List</a> `;
        //div.appendChild(innerDiv);
        console.log("div button to add",innerDiv);
        return innerDiv;
        //const aEdit = document.createElement('a');
        //.classList.add()
        //const aDelete = document.createElement('a');
        //.classList.add()
        //const aList = document.createElement('a');
        //.classList.add()

    },


    entryItemDetailsButtons: (input, criteria, id) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<a class="btn btn-outline-primary" href="/dex/u/${input}/p/${criteria}/Edit/ie/${id}">Edit</a> | 
             <a class="btn btn-outline-danger" href="/dex/u/${input}/p/${criteria}/Delete/ie/${id}">Delete</a> |
             <a class="btn btn-link" href="/dex/u/${input}/p/${criteria}/cont/list/soc">Back to List</a> `;
        //div.appendChild(innerDiv);
        console.log("div buttons to add", innerDiv);
        return innerDiv;
    },



    entryListButtons: (input, criteria, entryid) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.classList.add("nav");
        innerDiv.classList.add("operationLinks");
        innerDiv.innerHTML =
            `<a class="nav-item btn btn-sm btn-outline-success"      href="/dex/u/${input}/p/${criteria}/cont/soc/${entryid}">View</a> |
             <a class="nav-item btn btn-sm btn-outline-warning" href="/dex/u/${input}/p/${criteria}/Edit/ie/${entryid}">Edit</a> | 
             <a class="nav-item btn btn-sm btn-outline-danger"    href="/dex/u/${input}/p/${criteria}/Delete/ie/${entryid}">Delete</a>`;
        //div.appendChild(innerDiv);
        console.log("div buttons to add", innerDiv);
        return innerDiv;
    },


    socialMediaDetailsButtons: (input, criteria, id) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<a class="btn btn-outline-primary" href="/dex/u/${input}/p/${criteria}/Edit/ie/${id}">Edit</a> | 
             <a class="btn btn-outline-danger" href="/dex/u/${input}/p/${criteria}/Delete/ie/${id}">Delete</a> |
             <a class="btn btn-link" href="/dex/u/${input}/p/${criteria}/cont/list/soc">Back to List</a> `;
        //div.appendChild(innerDiv);
        console.log("div buttons to add", innerDiv);
        return innerDiv;
    },



    socialMediaListButtons: (input, criteria, entryid) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.classList.add("nav");
        innerDiv.classList.add("operationLinks");
        innerDiv.innerHTML =
            `<a class="nav-item btn btn-sm btn-outline-success"      href="/dex/u/${input}/p/${criteria}/cont/soc/${entryid}">View</a> |
             <a class="nav-item btn btn-sm btn-outline-warning" href="/dex/u/${input}/p/${criteria}/Edit/ie/${entryid}">Edit</a> | 
             <a class="nav-item btn btn-sm btn-outline-danger"    href="/dex/u/${input}/p/${criteria}/Delete/ie/${entryid}">Delete</a>`;
        //div.appendChild(innerDiv);
        console.log("div buttons to add", innerDiv);
        return innerDiv;
    },


    //TODO - verify these links
    // links to person associated entities
    personEntityButtons: (input, criteria, id) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<a class="btn btn-outline-primary" href="/dex/u/${input}/p/${criteria}/cont/list/soc">Entry Items</a> | 
             <a class="btn btn-outline-secondary" href="/dex/u/${input}/p/${criteria}/cont/list/soc/">Social Media</a> |
             <a class="btn btn-outline-dark" style="text-decoration:strike-through;" href="/dex/u/${input}/p/${criteria}/relations/ie/">Dependants</a> `;
        //div.appendChild(innerDiv);
        console.log("div buttons to add", innerDiv);
        return innerDiv;
    },





    userDetailsButtons: (input) => {

        //const div = document.getElementById(elementId);
        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<a class="btn btn-outline-primary" href="/dex/edit/${input}">Edit</a> | 
             <a class="btn btn-outline-danger" href="/dex/delete/${input}">Delete</a> |
             <!--disabled--> <a class="btn btn-link"  style="/*text-decoration:line-through;*/" href="/">Back To Home</a> `;
        //div.appendChild(innerDiv);
        console.log("div button to add", innerDiv);
        return innerDiv;
    }


}