"use strict";

import { DOM } from "./DOMCreator.js"; // import repository
import { UserRepository } from "./UserRepository.js"; // import repository
const userRepo = new UserRepository("https://localhost:7134/api/user"); // instantiate repository, set base address

//const f = new Date("2015-03-25T12:00:00-06:00");
//const d = f.getDate();
//const m = f.getMonth() + 1;
//const y = f.getFullYear();

//const date = new String(`${d}` + "-" + `${m}` + "-" + `${y}`);



const email = document.getElementById("email").value;
//DOM.logElementToConsole(email);
const dexResponse = await userRepo.readDex(email);
//console.log(dexResponse);


// TODO - Format Birthdays
const birthdayFormatted = dexResponse.DateOfBirth;
console.log(birthdayFormatted);


/*--------------------------------*/
async function standardFetchTest() {
    const response = await fetch(`https://localhost:7134/api/user/dex/${email}`);
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    else {
        console.log(response);
        const data = await response.json();
        console.log(data);
        console.log(data.Roles);
    }
}
/*--------------------------------*/


async function populateDetails(userRepo, DexHolderMiddleVM) {
    //let rootDiv = document.querySelector("#dexHolderDetailsPane");     //span -> append(textNode)

    //let topTable = rootDiv.querySelector("#tableTopUserInfo");     //span -> append(textNode)
    //let topTopRow = topTable.querySelector("#topTopRow");     //span -> append(textNode)

    //let topLeftCell = topTopRow.querySelector("#userDetailLeftColumn");     //span -> append(textNode)
    //let topRightCell = topTopRow.querySelector("#userDetailRightColumn");     //span -> append(textNode)

    //let topLeft = topLeftCell.querySelector("#userLeftDL");     //span -> append(textNode)
    //let ddUserName = topLeft.querySelector("#userNameField");       //dd -> append(h3)   -> append(textNode)
    //let ddUserEmail = topLeft.querySelector("#userEmailField");     //dd -> append(span) -> append(textNode)

    //let topRight = topRightCell.querySelector("#userRightDL");     //span -> append(textNode)
    //let ddDexId = topRight.querySelector("#dexIdField");             //dd -> append(h3)   -> append(textNode)

    //let midTable = rootDiv.querySelector("#tableMiddleDexHolder");     //span -> append(textNode)

    //let trName = midTable.querySelector("#nameRow");     //span -> append(textNode)
    //let tdName = trName.querySelector("#nameCell");     //span -> append(textNode)
    //let h3Name = tdName.querySelector("#nameHead");     //span -> append(textNode)


    //let trLower = midTable.querySelector("#lowerDetailsRow");     //span -> append(textNode)
    //let btmTable = trLower.querySelector("#tableBottom");     //span -> append(textNode)
    //let btmTr = btmTable.querySelector("#tableBottomRow");     //span -> append(textNode)

    //let cellGender = btmTr.querySelector("#genderCell");           //dd -> append(textNode)
    //let dlGender = cellGender.querySelector("#genderDL");           //dd -> append(textNode)
    //let cellPronouns = btmTr.querySelector("#pronounsCell");       //dd -> append(textNode)
    //let dlPronouns = cellPronouns.querySelector("#pronounsDL");       //dd -> append(textNode)
    //let cellBirthday = btmTr.querySelector("#birthdayCell");       //dd -> append(textNode)
    //let dlBirthday = cellBirthday.querySelector("#birthdayDL");       //dd -> append(textNode)
    //let cellPeopleCount = btmTr.querySelector("#peopleCountCell");       //dd -> append(textNode)
    //let dlPeopleCount = cellPeopleCount.querySelector("#peopleCountDL");       //dd -> append(textNode)

    //let ddPeopleCount = dlPeopleCount.querySelector("#peopleCountField"); //dd -> append(h4)   -> append(textNode)

    /*--------------------------*/
    //let h3DexId = ddDexId.querySelector("#dexIdHead");              //h3 -> append(textNode) 
    //let h3UserName = ddUserName.querySelector("#userNameHead");        //h3 -> append(textNode)
    //let h4PeopleCount = ddPeopleCount.querySelector("#peopleCountHead");  //h4 -> append(textNode)
    //let ddGender = dlGender.querySelector("#genderField");           //dd -> append(textNode)
    //let ddPronouns = dlPronouns.querySelector("#pronounsField");       //dd -> append(textNode)
    //let ddBirthday = dlBirthday.querySelector("#birthdayField");       //dd -> append(textNode)
    //let spanUserEmail = ddUserEmail.querySelector("#userEmailSpan");  //span -> append(textNode)

    //let spanFirst = h3Name.querySelector("#firstNameField");     //span -> append(textNode)
    //let spanMiddle = h3Name.querySelector("#middleNameField");   //span -> append(textNode)
    //let spanLast = h3Name.querySelector("#lastNameField");       //span -> append(textNode)
    /*--------------------------*/


    let h3DexId       = document.getElementById("dexIdHead");              //h3 -> append(textNode) 
    let h3UserName    = document.getElementById("userNameHead");        //h3 -> append(textNode)
    let h4PeopleCount = document.getElementById("peopleCountHead");  //h4 -> append(textNode)
    let ddGender      = document.getElementById("genderField");           //dd -> append(textNode)
    let ddPronouns    = document.getElementById("pronounsField");       //dd -> append(textNode)
    let ddBirthday    = document.getElementById("birthdayField");       //dd -> append(textNode)
    let spanUserEmail = document.getElementById("userEmailSpan");  //span -> append(textNode)
    let spanFirst     = document.getElementById("firstNameField");     //span -> append(textNode)
    let spanMiddle    = document.getElementById("middleNameField");   //span -> append(textNode)
    let spanLast      = document.getElementById("lastNameField");       //span -> append(textNode)


 


    let elemArray = [
        h3DexId,
        h3UserName,
        h4PeopleCount,
        ddGender,
        ddPronouns,
        ddBirthday,
        spanUserEmail,
        spanFirst,
        spanMiddle,
        spanLast
    ]

    let textArray = [
        DexHolderMiddleVM.DexId,
        DexHolderMiddleVM.ApplicationUserName,
        DexHolderMiddleVM.PeopleCount,
        DexHolderMiddleVM.Gender,
        DexHolderMiddleVM.Pronouns,
        DexHolderMiddleVM.DateOfBirth,
        DexHolderMiddleVM.ApplicationEmail,
        DexHolderMiddleVM.FirstName,
        DexHolderMiddleVM.MiddleName,
        DexHolderMiddleVM.LastName
    ]

    if (elemArray.length == textArray.length) {
        console.info("Beginning loop...");


        //console.log("people count: " , textArray[2]);
        if (elemArray[2] == "") { textArray[2] = 0; }
        //console.log("people count: ", textArray[2]);

        //console.log("HERE: application email: " , textArray[6]);


        for (let i = 0; i < elemArray.length; i++) {
            try {

                //console.log(`${textArray[i]} :`, elemArray[i])

                //console.info("Attempting to remove children...");
                try {
                    DOM.removeChildren(elemArray[i]);
                    console.log("Children Removed: ", elemArray[i])
                } catch (e) {
                    console.warn("ERROR: Could not remove element children! :", e)
                }


                //console.info("Setting element text directly...");
                DOM.setDirectElementTextWithPlaceholder(elemArray[i], textArray[i]);

                console.log("Element Text Set: ", elemArray[i]);
            } catch (e) {
                console.error(`An Error Occured within the For Loop!! - ${textArray[i]}`, e);
            }
        }
        console.info("loop complete!");

    }
    else {
        console.log("ERROR: element and text array must be the same length!!")
    }



    try {
        let tdRole = document.getElementById("userRolesCell");
        console.log(tdRole);
        try {
            DOM.removeChildren(tdRole);
        } catch (e) {
            console.log("ERROR: Could not remove children!!", tdRole, e)
        }
        const h5 = document.createElement('h5');
        h5.appendChild(document.createTextNode("Role(s):"));
        console.log(h5);
        tdRole.appendChild(h5);
        console.log(tdRole);
        const ulRole = DOM.createULfromArrayProperty(DexHolderMiddleVM.Roles, "RoleName")
        console.log(ulRole);
        tdRole.appendChild(ulRole);
        console.log(tdRole);
    } catch (e) {
        console.log("ERROR: something went wrong while making roles!!", e)

    }



}




populateDetails(userRepo, dexResponse);