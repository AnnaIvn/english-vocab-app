let examples = [];
let synonyms = [];
let antonyms = [];

// Validation function for allowed characters
function isValidInput(value) {
    let pattern = /^[a-zA-Z0-9\s.,!?'"():;_-]+$/;
    return pattern.test(value);
}

function addExample() {
    let value = document.getElementById("newExample").value.trim();

    if (!isValidInput(value)) {
        alert("Only English letters, numbers, and common punctuation are allowed.");
        return;
    }

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

function addSynonym() {
    let value = document.getElementById("newSynonym").value.trim();

    if (!isValidInput(value)) {
        alert("Only English letters, numbers, and common punctuation are allowed.");
        return;
    }

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

function addAntonym() {
    let value = document.getElementById("newAntonym").value.trim();

    if (!isValidInput(value)) {
        alert("Only English letters, numbers, and common punctuation are allowed.");
        return;
    }

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
