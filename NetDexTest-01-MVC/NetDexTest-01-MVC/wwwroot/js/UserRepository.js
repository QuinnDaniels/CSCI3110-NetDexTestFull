﻿"use strict";
export class UserRepository { //create a class and export it (for use in UserIndex)
    #baseAddress;
    constructor(baseAddress) {
        this.#baseAddress = baseAddress;    // constructor initializes private member, #baseAddress, to the incoming argument
    }



    /*--------how & where to set the address-----*/
    // in userListAll.js (the js for the page):
    //
    // import { UserRepository } from "./UserRepository.js"; // import repository
    //
    //const userRepo = new UserRepository("https://localhost:7134/api/user");
    //      // instantiate repository,^^                    ^^^
    //                              //  set base address   __|
    //
    /*-------------------------------------------*/




    //const userAddress = ``;

    /*---------------------------*/
    // ReadAll
    //Given: Nothing
    //Returns: Collection of objects
    //1. Send the GET request
    //2. Await response
    //3. Check for errors
    //4. Return the collection
    /*--------------------------*/
    /**/
    async readAll() {
        const address = `${this.#baseAddress}/admin/all`; //`https://localhost:7134/api/user/admin/all`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the boardgame data.");
        }
        return await response.json();
    }


    async readOneId(id) {
        //this.id = id;
        const address = `${this.#baseAddress}/one/${id}`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the boardgame data.");
        }
        return await response.json();
    }

    async readDex(input) {
        const address = `${this.#baseAddress}/dex/${input}`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the dex data.");
        }
        return await response.json();
    }
}