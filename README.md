# أكاديمية التميز

## إعداد أول مدير

لا تُحفظ بيانات المدير في Git. من مجلد المشروع شغّل:

```powershell
dotnet user-secrets set "Admin:Email" "admin@example.com"
dotnet user-secrets set "Admin:Password" "Choose-a-strong-password-123!"
```

ثم أعد تشغيل الموقع. يُنشأ المستخدم ويُمنح دور `Admin` تلقائيًا، وبعدها سجّل الدخول من `/Identity/Account/Login` وافتح `/Admin`.
