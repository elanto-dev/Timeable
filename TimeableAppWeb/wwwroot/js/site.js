// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// https://www.youtube.com/watch?v=ApdS6OThm_c

function loadTeacherClickEventsForSubjectView(teacherCount, teacherName, teacherRole, removeTeacher) {
    var id = `teacher-${teacherCount}`;
    var div = $("<div />").attr("id", id);
    var value = "";
    var innerNameDiv = $("<div />").attr("class", "form-group");
    var fullNameLabel = $("<label />").attr("class", "control-label")
        .attr("name", "Teachers[" + teacherCount + "].FullName");
    var fullNameInput = $("<input />").attr("type", "text").attr("class", "form-control").attr("name", "Teachers[" + teacherCount + "].FullName");
    var innerRoleDiv = $("<div />").attr("class", "form-group");
    var roleLabel = $("<label />").attr("class", "control-label")
        .attr("name", "Teachers[" + teacherCount + "].FullName");
    var roleInput = $("<input />").attr("type", "text").attr("class", "form-control").attr("name", "Teachers[" + teacherCount + "].Role");

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
};