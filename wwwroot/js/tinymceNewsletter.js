tinymce.init({
    selector: '#editor2',  // change this value according to your HTML
    plugin: 'a_tinymce_plugin',
    a_plugin_option: true,
    a_configuration_option: 400
});



//tinymce.init({
//    selector: '#editor2',
//    language: 'pl',
//    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
//    /*    document_base_url: system_url,*/
//    menubar: false,
//    statusbar: false,
//    toolbar: false,
//    relative_urls: false,
//    remove_script_host: false,
//    convert_urls: true,
//    content_css: 'css/Newsletter/newsletter.css',
//    plugins: 'image anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tableofcontents footnotes mergetags autocorrect image code',
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
//    height: 200,
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
//    selector: '#editor2',
//    language: 'pl',
//    menubar: false,
//    statusbar: false,
//    toolbar: false,
//    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
//    relative_urls: true,
//    document_base_url: 'https://partneralluro.hostingasp.pl/',
//    tinycomments_mode: 'embedded'
//});