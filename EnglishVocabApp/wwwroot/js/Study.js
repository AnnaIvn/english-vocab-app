let words = []; 
let currentIndex = 0;
let isFlipped = false;

function flipCard() {
    let card = document.getElementById("word-card");
    if (isFlipped) {
        card.textContent = words[currentIndex].Name;
    } else {
        card.textContent = words[currentIndex].Meaning;
    }
    isFlipped = !isFlipped;
}

function prevWord() {
    if (currentIndex > 0) {
        currentIndex--;
        updateCard();
    }
}

function nextWord() {
    if (currentIndex < words.length - 1) {
        currentIndex++;
        updateCard();
    }
}

function updateCard() {
    isFlipped = false;
    document.getElementById("word-card").textContent = words[currentIndex].Name;
    document.getElementById("prev-btn").disabled = currentIndex === 0;
    document.getElementById("next-btn").disabled = currentIndex === words.length - 1;
}

function initializeWords(jsonData) {
    words = jsonData;
    updateCard();
}

window.flipCard = flipCard;
window.prevWord = prevWord;
window.nextWord = nextWord;
window.initializeWords = initializeWords;