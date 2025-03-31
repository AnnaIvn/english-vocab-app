let examples = [];
let synonyms = [];
let antonyms = [];

//function addExample() {
//    let value = document.getElementById("newExample").value;
//    if (value.trim()) {
//        examples.push(value);
//        updateExamplesList();
//        document.getElementById("newExample").value = "";
//    }
//}

function addExample() {
    let value = document.getElementById("newExample").value.trim();

    if (value && !examples.includes(value)) {
        examples.push(value);
        updateExamplesList();
        document.getElementById("newExample").value = "";
    } else {
        alert("This example is already added. Use another one instead."); 
    }
}


function removeExample(value) {
    examples = examples.filter(item => item !== value);
    updateExamplesList();
}

function updateExamplesList() {
    document.getElementById("examplesList").innerHTML = examples.map(item =>
        `<li>${item} <button type="button" onclick="removeExample('${item}')" class="remove-btn"></button></li>`).join("");
    document.getElementById("examplesInput").value = examples.join(";");
}

//function addSynonym() {
//    let value = document.getElementById("newSynonym").value;
//    if (value.trim()) {
//        synonyms.push(value);
//        updateSynonymsList();
//        document.getElementById("newSynonym").value = "";
//    }
//}

function addSynonym() {
    let value = document.getElementById("newSynonym").value.trim();

    if (value && !synonyms.includes(value)) {
        synonyms.push(value);
        updateSynonymsList();
        document.getElementById("newSynonym").value = "";
    } else {
        alert("This synonym is already added. Use another one instead.");
    }
}

function removeSynonym(value) {
    synonyms = synonyms.filter(item => item !== value);
    updateSynonymsList();
}

function updateSynonymsList() {
    document.getElementById("synonymsList").innerHTML = synonyms.map(item =>
        `<li>${item} <button type="button" onclick="removeSynonym('${item}')" class="remove-btn"></button></li>`).join("");
    document.getElementById("synonymsInput").value = synonyms.join(", ");
}

//function addAntonym() {
//    let value = document.getElementById("newAntonym").value;
//    if (value.trim()) {
//        antonyms.push(value);
//        updateAntonymsList();
//        document.getElementById("newAntonym").value = "";
//    }
//}

function addAntonym() {
    let value = document.getElementById("newAntonym").value.trim();

    if (value && !antonyms.includes(value)) {
        antonyms.push(value);
        updateAntonymsList();
        document.getElementById("newAntonym").value = "";
    } else {
        alert("This antonym is already added. Use another one instead.");
    }
}

function removeAntonym(value) {
    antonyms = antonyms.filter(item => item !== value);
    updateAntonymsList();
}

function updateAntonymsList() {
    document.getElementById("antonymsList").innerHTML = antonyms.map(item =>
        `<li>${item} <button type="button" onclick="removeAntonym('${item}')" class="remove-btn"></button></li>`).join("");
    document.getElementById("antonymsInput").value = antonyms.join(", ");
}

function updateHiddenFields() {
    document.getElementById("examplesInput").value = examples.join(";");
    document.getElementById("synonymsInput").value = synonyms.join(", ");
    document.getElementById("antonymsInput").value = antonyms.join(", ");
}