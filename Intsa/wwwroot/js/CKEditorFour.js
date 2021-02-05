window.CKEditorFour = (() => {
    const editors = {};

    return {
        init(id, dotNetReference) {
            var editor = CKEDITOR.replace(document.getElementById(id));
            editors[id] = editor;
            editor.on('change', function (evt) {
                var data = evt.editor.getData();
                dotNetReference.invokeMethodAsync('EditorFourDataChanged', data);
            });
        },
        destroy(id) {
            editors[id].destroy()
                .then(() => delete editors[id])
                .catch(error => console.log(error));
        }
    };
})(); 