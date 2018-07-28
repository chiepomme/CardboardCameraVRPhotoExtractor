# CardboardCameraVRPhotoExtractor
Extract Mirage Solo's side by side photos

画像の仕様はこちらの Cardboard Camera VR Photo Format と同じようです。  
[Cardboard Camera VR Photo Format  \|  Google VR  \|  Google Developers](https://developers.google.com/vr/reference/cardboard-camera-vr-photo-format)

XMP のメタデータの中に右目用の画像がそのまままるっと BASE64 で入っているというフォーマットです。
注意としては BASE64 といいつつパディングがない状態で来るので、自分でパディングを追加してあげないといけません。

なお、ウェブサービスなど他の抽出方法はこちらにまとまっています。  
[Editing VR180\-photos from Mirage camera \- different attempts : virtualreality](https://www.reddit.com/r/virtualreality/comments/8w9qch/editing_vr180photos_from_mirage_camera_different/)
