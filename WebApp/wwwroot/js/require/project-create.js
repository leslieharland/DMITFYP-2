$(function () {
    var punctRE = /[\u2000-\u206F\u2E00-\u2E7F\\'!"#\$%&\(\)\*\+,\-\.\/:;<=>\?@\[\]\^_`\{\|\}~]/g;
    var frmStudentInitiated = $('.student-initiated');
    var customcontent = $('#custom-content');
    $.fn.editable.defaults.mode = 'inline';

    $('#add-field').click(function (e) {
        e.preventDefault();
        var count = $('#add-field').val();
        $('#add-field').val(++count);


        if (count <= 10) {
            $('<button class="removeFromView" onclick="removeItem(this); return false;"></button><a href="#" data-id="0" id="fields' + count + '__label" data-type="text">Your field name</a>').appendTo(customcontent);
            $('<textarea class="fieldValue" name="optionfv" tabindex="1" id="fields' + count + '__value" autofocus="autofocus" data-val="true"></textarea>').appendTo(customcontent);
            $("#fields" + count + "__label").editable({
                validate: function (value) {
                    if (punctRE.test(value)) {
                        return 'No punctuations allowed';
                    }
                }
            });
            $('#FieldCount').val(count);
        } else {
            alert('Maximum fields reached');
        }
    });

    var fielditems = sessionStorage.getItem("unsavedfields");

    if ($('.field-validation-error').length && fielditems != "undefined") {
        customcontent.append(sessionStorage.getItem("unsavedfields"));
        $("a[data-type='text']").each(function (idx) {
            $(this).editable({
                validate: function (value) {
                    if (punctRE.test(value)) {
                        return 'No punctuations allowed';
                    }
                }
            });
        });
        sessionStorage.removeItem("unsavedfields");
    }

});

function saveappendcontent() {
    sessionStorage.setItem("unsavedfields", $('#custom-content').html());
    var fieldsJson = [];
    function Field(label, value, id) {
        this.id = id;
        this.label = label;
        this.value = value;
    }
    $("a[data-type='text']").each(function (idx) {
        var fieldLabel = new Field($(this).text(), null, $(this).attr('data-id'));
        fieldsJson.push(fieldLabel);
    });

    $('textarea.fieldValue').each(function (idx) {
        var text = $(this).val();
        fieldsJson[idx].value = text;
    });

    $('#displayFields').val(JSON.stringify(fieldsJson));
}


function removeItem(elem) {
    var anchorObj = $(elem).next();
    $(anchorObj).next().remove();
    $(anchorObj).remove();    
    $(elem).remove();
}