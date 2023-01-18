
tinymce.init({
    selector: '#editorNewsletter2',
    language: 'pl',
    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
    tinydrive_token_provider: '7jymcsfhr21jkg9tegltt4mduk03g8tkuk3zgof7cwr9i3i5',
    /*    document_base_url: system_url,*/
    relative_urls: false,
    promotion: false,
    remove_script_host: false,
    convert_urls: true,
    /*content_css: 'css/Newsletter/newsletter.css',*/
    plugins: 'image code template print preview powerpaste casechange importcss tinydrive searchreplace autolink autosave save directionality advcode visualblocks visualchars fullscreen image link media mediaembed template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons advtable export',
    toolbar: ["undo redo | blocks fontfamily fontsize | bold italic underline strikethrough",
        "|forecolor backcolor | | addcomment showcomments | spellcheckdialog a11ycheck | align lineheight | numlist bullist indent outdent | emoticons charmap | removeformat",
        "| link image media table mergetags |"],
    image_list: [
        { title: 'My image', value: '' },
        { title: 'My image', value: '' }
    ],
    templates: [
        { title: 'Szablon1', description: 'Szablon1', url: '\\templates\\template0.html' },
        { title: 'Szablon2', description: 'Szablon2', url: '\\templates\\template2.html' }
    ],
    images_upload_url: '/upload', // this is the upload target URL - our first endpoint
    image_list: '/filelist', // the list of the media files, our second endpoint
    // enable title field in the Image dialog
    image_title: true,
    // enable automatic uploads of images represented by blob or data URIs
    automatic_uploads: true,
    height: "700",
    setup: function (editor) {

        editor.ui.registry.addButton('customInsertButton', {
            text: 'Wstaw produktyTEST',
            onAction: function (_) {

                editor.insertContent('&nbsp;<strong>It\'s my button!</strong>&nbsp;');

            }
        });

        var toTimeHtml = function (date) {
            return '<time datetime="' + date.toString() + '">' + date.toDateString() + '</time>';
        };

        editor.ui.registry.addButton('customDateButton', {
            icon: 'insert-time',
            tooltip: 'Insert Current Date',
            disabled: true,
            onAction: function (_) {
                editor.insertContent(toTimeHtml(new Date()));
            },
            onSetup: function (buttonApi) {
                var editorEventCallback = function (eventApi) {
                    buttonApi.setDisabled(eventApi.element.nodeName.toLowerCase() === 'time');
                };
                editor.on('NodeChange', editorEventCallback);

                /* onSetup should always return the unbind handlers */
                return function (buttonApi) {
                    editor.off('NodeChange', editorEventCallback);
                };
            }
        });
    },
    // add custom filepicker only to Image dialog
    image_caption: true,
    // add custom filepicker only to Image dialog
    file_picker_types: 'image',
    file_picker_callback: function (cb, value, meta) {
        var input = document.createElement('input');
        input.setAttribute('type', 'file');
        input.setAttribute('accept', 'image/*');

        input.onchange = function () {
            var file = this.files[0];
            var reader = new FileReader();

            reader.onload = function () {
                var id = 'blobid' + (new Date()).getTime();
                var blobCache = tinymce.activeEditor.editorUpload.blobCache;
                var base64 = reader.result.split(',')[1];
                var blobInfo = blobCache.create(id, file, base64);
                blobCache.add(blobInfo);

                // call the callback and populate the Title field with the file name
                cb(blobInfo.blobUri(), { title: file.name });
            };
            reader.readAsDataURL(file);
        };

        input.click();
    }
});

//tinymce.init({
//    selector: '#editorNewsletter',
//    toolbar: 'customInsertButton customDateButton',
//    setup: function (editor) {

//        editor.ui.registry.addButton('customInsertButton', {
//            text: 'My Button',
//            onAction: function (_) {
//                editor.insertContent('&nbsp;<strong>It\'s my button!</strong>&nbsp;');
//            }
//        });

//        var toTimeHtml = function (date) {
//            return '<time datetime="' + date.toString() + '">' + date.toDateString() + '</time>';
//        };

//        editor.ui.registry.addButton('customDateButton', {
//            icon: 'insert-time',
//            tooltip: 'Insert Current Date',
//            disabled: true,
//            onAction: function (_) {
//                editor.insertContent(toTimeHtml(new Date()));
//            },
//            onSetup: function (buttonApi) {
//                var editorEventCallback = function (eventApi) {
//                    buttonApi.setDisabled(eventApi.element.nodeName.toLowerCase() === 'time');
//                };
//                editor.on('NodeChange', editorEventCallback);

//                /* onSetup should always return the unbind handlers */
//                return function (buttonApi) {
//                    editor.off('NodeChange', editorEventCallback);
//                };
//            }
//        });
//    },
//    content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
//});



//tinymce.init({
//    selector: '#editorNewsletter2',
//    language: 'pl',
//    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
//    /*    document_base_url: system_url,*/
//    relative_urls: false,
//    remove_script_host: false,
//    convert_urls: true,
//    content_css: 'css/Newsletter/newsletter.css',
//    plugins: 'template, image anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tableofcontents footnotes mergetags autocorrect image code',
//    toolbar1: 'customInsertButton customDateButton undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck',
//    toolbar2: 'image | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat ',
//    image_list: [
//        { title: 'My image', value: '' },
//        { title: 'My image', value: '' }
//    ],
//    images_upload_url: '/upload', // this is the upload target URL - our first endpoint
//    image_list: '/filelist', // the list of the media files, our second endpoint
//    // enable title field in the Image dialog
//    image_title: true,
//    // enable automatic uploads of images represented by blob or data URIs
//    automatic_uploads: true,
//    height: "700",
//    setup: function (editor) {

//        editor.ui.registry.addButton('customInsertButton', {
//            text: 'Wstaw produktyTEST',
//            onAction: function (_) {

//                editor.insertContent('&nbsp;<strong>It\'s my button!</strong>&nbsp;');

//            }
//        });

//        var toTimeHtml = function (date) {
//            return '<time datetime="' + date.toString() + '">' + date.toDateString() + '</time>';
//        };

//        editor.ui.registry.addButton('customDateButton', {
//            icon: 'insert-time',
//            tooltip: 'Insert Current Date',
//            disabled: true,
//            onAction: function (_) {
//                editor.insertContent(toTimeHtml(new Date()));
//            },
//            onSetup: function (buttonApi) {
//                var editorEventCallback = function (eventApi) {
//                    buttonApi.setDisabled(eventApi.element.nodeName.toLowerCase() === 'time');
//                };
//                editor.on('NodeChange', editorEventCallback);

//                /* onSetup should always return the unbind handlers */
//                return function (buttonApi) {
//                    editor.off('NodeChange', editorEventCallback);
//                };
//            }
//        });
//    },
//    // add custom filepicker only to Image dialog
//    image_caption: true,
//    // add custom filepicker only to Image dialog
//    file_picker_types: 'image',
//    file_picker_callback: function (cb, value, meta) {
//        var input = document.createElement('input');
//        input.setAttribute('type', 'file');
//        input.setAttribute('accept', 'image/*');

//        input.onchange = function () {
//            var file = this.files[0];
//            var reader = new FileReader();

//            reader.onload = function () {
//                var id = 'blobid' + (new Date()).getTime();
//                var blobCache = tinymce.activeEditor.editorUpload.blobCache;
//                var base64 = reader.result.split(',')[1];
//                var blobInfo = blobCache.create(id, file, base64);
//                blobCache.add(blobInfo);

//                // call the callback and populate the Title field with the file name
//                cb(blobInfo.blobUri(), { title: file.name });
//            };
//            reader.readAsDataURL(file);
//        };

//        input.click();
//    }
//});




//tinymce.init({
//    selector: 'textarea',
//    language: 'pl',
//    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
//    relative_urls: true,
//    document_base_url: 'https://partneralluro.hostingasp.pl/',
//    plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect',
//    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
//    tinycomments_mode: 'embedded'
//});