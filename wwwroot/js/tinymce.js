tinymce.init({
    selector: 'textarea',
    language: 'pl',
    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
    relative_urls: true,
    document_base_url: 'https://partneralluro.hostingasp.pl/',
    plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect',
    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
    tinycomments_mode: 'embedded'
});


tinymce.init({
    selector: '#textarea',
    language: 'pl',
    language_url: 'https://partneralluro.hostingasp.pl/js/tinymce/langs/pl.js',  // site absolute URL
    /*    document_base_url: system_url,*/
    relative_urls: false,
    remove_script_host: false,
    convert_urls: true,
    content_css: 'css/Newsletter/newsletter.css',
    plugins: 'image anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tableofcontents footnotes mergetags autocorrect image code',
    toolbar1: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck',
    toolbar2: 'image | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat ',
    image_list: [
        { title: 'My image', value: '' },
        { title: 'My image', value: '' }
    ],
    images_upload_url: '/upload', // this is the upload target URL - our first endpoint
    image_list: '/filelist', // the list of the media files, our second endpoint
    // enable title field in the Image dialog
    image_title: true,
    // enable automatic uploads of images represented by blob or data URIs
    automatic_uploads: true,
    height: 2000,
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