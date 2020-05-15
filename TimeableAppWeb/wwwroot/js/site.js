// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// https://www.youtube.com/watch?v=ApdS6OThm_c

function loadTeacherClickEventsForSubjectView(teacherCount, teacherName, teacherRole, removeTeacher) {
    var id = `teacher-${teacherCount}`;
    var div = $("<div />").attr("id", id).attr("class", "teacher-div");;
    var value = "";
    var innerNameDiv = $("<div />").attr("class", "form-group");
    var fullNameLabel = $("<label />").attr("class", "control-label teacher-name-label")
        .attr("name", "Teachers[" + teacherCount + "].TeacherName");
    var fullNameInput = $("<input />").attr("type", "text").attr("class", "form-control teacher-name-input")
        .attr("name", "Teachers[" + teacherCount + "].TeacherName");
    var innerRoleDiv = $("<div />").attr("class", "form-group");
    var roleLabel = $("<label />").attr("class", "control-label teacher-role-label")
        .attr("name", "Teachers[" + teacherCount + "].TeacherRole");
    var roleInput = $("<input />").attr("type", "text").attr("class", "form-control teacher-role-input")
        .attr("name", "Teachers[" + teacherCount + "].TeacherRole");

    fullNameLabel.text(teacherName);
    roleLabel.text(teacherRole);
    fullNameInput.val(value);
    roleInput.val(value);

    innerNameDiv.append(fullNameLabel);
    innerNameDiv.append(fullNameInput);
    innerRoleDiv.append(roleLabel);
    innerRoleDiv.append(roleInput);

    div.append(innerNameDiv);
    div.append(innerRoleDiv);

    var innerButtonDiv = $("<div />").attr("class", "form-group");
    var button = $("<input />").attr("type", "button").attr("value", removeTeacher).attr("class", "btn btn-outline-danger");
    button.click(function () {
        $(`#${id}`).remove();
    });
    innerButtonDiv.append(button);
    div.append(innerButtonDiv);
    $("#teachers-list").append(div);
}


function RemoveTeacher(id) {
    $(`#${id}`).remove();
    var i = 0;
    alert($(".teacher-div").length);
    $(".teacher-div").each(function () {
        alert($(this).attr('id'));
        var newId = `teacher-${i}`;
        $(this).attr("id", newId);
        alert($(this).attr('id'));
        i++;
    });
    i = 0;
    $(".teacher-name-label").each(function () {
        $(this).attr("name", `Teachers[${i}].TeacherName`);
        i++;
    });
    i = 0;
    $(".teacher-name-input").each(function () {
        $(this).attr("name", `Teachers[${i}].TeacherName`);
        i++;
    });
    i = 0;
    $(".teacher-role-label").each(function () {
        $(this).attr("name", `Teachers[${i}].TeacherRole`);
        i++;
    });
    i = 0;
    $(".teacher-input-input").each(function () {
        $(this).attr("name", `Teachers[${i}].TeacherRole`);
        i++;
    });
};