var Site = new Class.create({

  previousValue: '',
  previousType: -1,
  updateUrl: '',

  initialize: function(updateUrl) {
    new PeriodicalExecuter(this.process.bind(this), 2);
    this.updateUrl = updateUrl;
    this.previousValue = '';
    this.previousType = -1;
    $('type')
      .observe("change", this.process.bind(this))
    $('source')
      .observe("change", this.process.bind(this))
      .observe("keyup", this.resizeTextarea)
      .observe("keypress", this.resizeTextarea)
    .focus();
  },

  process: function() {
    var newValue = $("source").value;
    var newType = $("type").value;
    if (newValue != this.previousValue || newType != this.previousType) {
      if (!newValue) {
        Element.update("destination", "");
      } else {
        new Ajax.Request(this.updateUrl, {
          parameters: { source: newValue, type: newType }
        });
      }
      this.previousValue = newValue;
      this.previousType = newType;
    }
  },

  // based on http://blog.bigsmoke.us/2007/02/18/automatic-html-textarea-resizing
  resizeTextarea: function() {
    var textArea = $('source');
    if (!textArea.originalRowCount) {
      textArea.originalRowCount = textArea.rows;
    }
    var rows = textArea.value.split('\n');
    var neededRows = 1;
    for (var i = 0; i < rows.length; i++) {
      var row = rows[i];
      neededRows += (row.length > textArea.cols) ? Math.floor(row.length / textArea.cols) : 1;
    }
    textArea.rows = Math.max(neededRows, textArea.originalRowCount);
  }
});