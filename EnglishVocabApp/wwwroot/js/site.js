// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function addEntity(url) {
    $.ajax({
        url: url,
        type: "GET",
        processData: false,
        contentType: false,
        data: null,
        async: true,
        error: function (jqxhr, textStatus, errorThrown) {
            console.error('Error', jqxhr);
            $('.modal').remove();
            $('.modal-backdrop').remove();
            bootbox.alert(jqxhr.responseText);
        }
    }).done((data) => {
        $('#modalX').empty();
        $('#modalX').append(data);
        $('.modal').modal('show');
    });
}

function addEntityWord(url) {
    $.ajax({
        url: url,
        type: "GET",
        processData: false,
        contentType: false,
        error: function (jqxhr) {
            console.error('Error', jqxhr);
            $('.modal').remove();
            $('.modal-backdrop').remove();
            bootbox.alert(jqxhr.responseText);
        }
    }).done((data) => {
        $('#modalX').empty().append(data);

        // Extract the selected word type from the modal's hidden field
        var selectedType = $("#TypeName").attr("data-selected");

        // Fetch WordTypes and populate the dropdown
        $.get("/Words/GetWordTypes", function (wordTypes) {
            var dropdown = $("#TypeName");
            dropdown.empty();
            dropdown.append('<option value=""></option>'); // Empty default option

            $.each(wordTypes, function (index, type) {
                dropdown.append('<option value="' + type + '">' + type + '</option>');
            });

            // Ensure correct selection
            if (selectedType) {
                dropdown.val(selectedType);
            }
        });

        $('.modal').modal('show');
    });
}

function editEntityWord(url) {
    $.ajax({
        url: url,
        type: "GET",
        processData: false,
        contentType: false,
        error: function (jqxhr) {
            console.error('Error', jqxhr);
            $('.modal').remove();
            $('.modal-backdrop').remove();
            bootbox.alert(jqxhr.responseText);
        }
    }).done((data) => {
        $('#modalX').empty().append(data);
        $('.modal').modal('show');

        // Small delay to ensure modal content is loaded
        setTimeout(() => {
            var selectedType = $("#TypeName").attr("data-selected"); // Retrieve correctly

            $.get("/Words/GetWordTypes", function (wordTypes) {
                var dropdown = $("#TypeName");
                dropdown.empty();
                dropdown.append('<option value=""></option>');

                $.each(wordTypes, function (index, type) {
                    dropdown.append('<option value="' + type + '">' + type + '</option>');
                });

                if (selectedType) {
                    dropdown.val(selectedType);
                }
            });
        }, 200);
    });
}

function deleteEntityWord(url) {
    $.ajax({
        url: url,
        type: "GET",
        processData: false,
        contentType: false,
        error: function (jqxhr, textStatus, errorThrown) {
            console.error('Error', jqxhr);
            $('.modal').remove();
            $('.modal-backdrop').remove();
            bootbox.alert(jqxhr.responseText);
        }
    }).done((data) => {
        $('#modalX').empty();
        $('#modalX').append(data);
        $('.modal').modal('show');
    });
}

